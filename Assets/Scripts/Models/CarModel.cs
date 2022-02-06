using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ExampleGB
{
    public sealed class CarModel : BaseSceneObject, IDisposable
    {
        private CarSObject _carSObject;
        private SelectedCarData _selectedCarData;
        //[SerializeField] private ParticleHelper _deathFire;
        [SerializeField] private Transform _centerOfMass;
        //private HealthBarUI _healthBar;
        //private AudioHelper _audio;

        private Wheel[] _wheels;

        [SerializeField] private LayerMask WhatIsSolid;

        [SerializeField] private float _motorTorque { get; set; }
        [SerializeField] private float _steer { get; set; }
        [SerializeField] private float _throttle { get; set; }

        private float _maxSteer = 1400.0f;
        private float _jumpForce = 300.0f;
        private float _KPHcoefficient = 3.6f;
        private float _baseDamage = 10.0f;
        [SerializeField] private float _speedDamage;

        private float _firstGearForce;
        private float _secondGearForce;
        private float _thirdGearForce;
        private float _backGearForce;

        private float _firstGearMagnitude = 20.0f;
        private float _SecondGearMagnitude = 50.0f;
        private float _thirdGearMagnitude = 80.0f;

        private float _firstGearPitch = 1.0f;
        private float _SecondGearPitch = 1.1f;
        private float _thirdGearPitch = 1.2f;
        private float _backGearPitch = 0.8f;

        private float _airTimer = 0.0f;
        private float _groundCheckDistance = 0.6f;

        private const string _audioPitch = "Idle";

        [SerializeField] private bool _inAir;
        [SerializeField] private bool _isGrounded;

        public bool IsDead = false;

        public float Speed;
        public float Health;
        public float MaxHealth = 500;

        public Transform MouseLookAt;

        public void FirstStarting(CarSObject carSObject, SelectedCarData carData)
        {
            _carSObject = carSObject;
            _selectedCarData = carData;
            _motorTorque = _carSObject.MotorTorque;
            _firstGearForce = _carSObject.FirstGear;
            _secondGearForce = _carSObject.SecondGear;
            _thirdGearForce = _carSObject.ThirdGear;
            _backGearForce = _carSObject.BackGear;
            Rigidbody.centerOfMass = _centerOfMass.localPosition;
            _wheels = GetComponentsInChildren<Wheel>();
            Health = MaxHealth;
            //_healthBar = FindObjectOfType<HealthBarUI>();
            //_healthBar.SetMaxHealth(Health);
            //_audio = GetComponent<AudioHelper>();
            _selectedCarData.OnThrottleAndSteer += ThrottleAndSteerChanger;
        }

        public void Ride()
        {
            Speed = Rigidbody.velocity.magnitude * _KPHcoefficient;
            _speedDamage = _baseDamage + Rigidbody.velocity.magnitude;    
        }

        public void FixedRide()
        {
            foreach (var wheel in _wheels)
            {
                wheel.SteerAngle = _steer * _maxSteer * Time.fixedDeltaTime;
                wheel.Torque = _throttle * _motorTorque * Time.fixedDeltaTime;
            }

            GearShifter(Speed);

            AirRotation();
        }

        public void ReturnOnWheels()
        {
            Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
            _airTimer = 2.0f;
            _inAir = true;
        }

        public void BrakesActivation(bool IsActivated)
        {
            foreach (var wheel in _wheels)
            {
                wheel.Breakes = IsActivated;
            }
        }

        public void CarHealth(InfoCollision info)
        {

        }

        public void ThrottleAndSteerChanger(float throttle, float steer)
        {
            if (!Groundcheck()) return;
            
            _throttle = throttle;
            _steer = steer;
        }

        public void Dispose()
        {
            _selectedCarData.OnThrottleAndSteer -= ThrottleAndSteerChanger;
        }

        private void OnEnable()
        {
            var corpse = GetComponentInChildren<CarView>();
            if (corpse != null) corpse.CarDestruction += CarHealth;
        }

        private void OnDisable()
        {
            var corpse = GetComponentInChildren<CarView>();
            if (corpse != null) corpse.CarDestruction -= CarHealth;
        }

        private void GearShifter(float speedValue)
        {
            if (_throttle > 0)
            {
                if (speedValue > 0 && speedValue < _firstGearMagnitude) AddGearForce(_firstGearForce, _firstGearPitch);
                if (speedValue > _firstGearMagnitude && speedValue < _SecondGearMagnitude) AddGearForce(_secondGearForce, _SecondGearPitch);
                if (speedValue > _SecondGearMagnitude && speedValue < _thirdGearMagnitude) AddGearForce(_thirdGearForce, _thirdGearPitch);
            }

            if (_throttle < 0) AddGearForce(_backGearForce, _backGearPitch);
        }

        private void AddGearForce(float force, float pitch)
        {
            Rigidbody.AddForce(transform.forward * force, ForceMode.Acceleration);
            //_audio.Pitch(_audioPitch, pitch);
        }

        private bool Groundcheck()
        {
            if (Physics.Raycast(_centerOfMass.transform.position, Vector3.down, out RaycastHit hit, _groundCheckDistance, WhatIsSolid))
            {
                Debug.DrawRay(_centerOfMass.transform.position, Vector3.down * _groundCheckDistance, Color.red);
                if (hit.collider != null) _isGrounded = true;
            }
            else _isGrounded = false;

            if (_airTimer >= 0)
            {
                _airTimer -= Time.deltaTime;
            }
            else
            {
                _inAir = false;
            }

            return _isGrounded;
        }        

        private void AirRotation()
        {
            if (_inAir)
            {
                var eulers = Vector3.zero;
                eulers.z += 2;
                Transform.Rotate(eulers);
            }
        }        

        private void OnCollisionEnter(Collision collision)
        {
            var setDamage = collision.gameObject.GetComponent<ICollision>();

            if (setDamage != null)
            {
                setDamage.OnCollision(new InfoCollision(_speedDamage, Transform));
            }
        }
    }
}
