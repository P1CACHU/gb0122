using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;


namespace ExampleGB
{
    [RequireComponent(typeof(UpdateManager))]
    [RequireComponent(typeof(PoolObjects))]
    public sealed class MainController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private CarSObject _carSettings;
        [SerializeField] private SelectedCarData _createdCarData;
        [SerializeField] private CinemachineFreeLook _camera;
        private PoolObjects _pool;

        private List<IController> _controllers;


        private void Awake()
        {            
            _controllers = new List<IController>();
            _pool = GetComponent<PoolObjects>();
        }

        private void Start()
        {          
            CreateControllers();
            StartControllers();

            _camera.Follow = _createdCarData.Value.MouseLookAt;
            _camera.LookAt = _createdCarData.Value.MouseLookAt;
        }

        private void CreateControllers()
        {
            _controllers.Add(new InputController(_createdCarData));
            _controllers.Add(new CarController(_spawnPositions, _carSettings, _createdCarData, _pool));            
        }

        private void StartControllers()
        {
            foreach (var controller in _controllers)
            {
                controller.Initialize();
            }            
        }
    }
}
