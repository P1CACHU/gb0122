using System;
using UnityEngine;


namespace ExampleGB
{
    public sealed class InputController : IController, IDisposable
    {   
        private KeyCode _activeFlashLight = KeyCode.F;
        private KeyCode _cancel = KeyCode.Escape;
        private KeyCode _reloadClip = KeyCode.R;
        private KeyCode _returnOnWheels = KeyCode.G;
        private KeyCode _breakes = KeyCode.Space;

        private SelectedCarData _selectedCarData;
        private CarModel _currentCar;

        private int _leftMB = 0;
        private int _rightMB = 1;
        private int _selectedWeapon = 0;

        private string _inputSteerAxis = "Horizontal";
        private string _inputThrottleAxis = "Vertical";

        public float ThrottleInput { get; private set; }
        public float SteerInput { get; private set; }

        public InputController(SelectedCarData car)
        {
            _selectedCarData = car;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }  

        public void Initialize()
        {
            _selectedCarData.OnSelected += GetSelectedCar;
            UpdateManager.SubscribeToUpdate(Execute);
        }

        public void Execute()
        {
            SteerInput = Input.GetAxis(_inputSteerAxis);
            ThrottleInput = Input.GetAxis(_inputThrottleAxis);
            _selectedCarData.SetInputValues(ThrottleInput, SteerInput);

            if (Input.GetKey(_breakes))
            {
                _currentCar.BrakesActivation(true);
            }
            else _currentCar.BrakesActivation(false);

            if (Input.GetKeyDown(_returnOnWheels))
            {
                _currentCar.ReturnOnWheels();
            }
        }        

        public void Dispose()
        {
            _selectedCarData.OnSelected -= GetSelectedCar;
            UpdateManager.UnsubscribeFromUpdate(Execute);
        }

        private void GetSelectedCar(CarModel car)
        {
            _currentCar = car;
        }
    }
}
