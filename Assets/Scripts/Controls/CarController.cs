using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ExampleGB
{
    public class CarController : IController, IDisposable
    {
        private List<Transform> _spawnPositions;
        private CarSObject _carSettings;
        private SelectedCarData _createdCarData;
        private CarModel _currentCar;
        private PoolObjects _pool;

        public CarController(List<Transform> spawnPositions, CarSObject carSettings, SelectedCarData carData, PoolObjects pool)
        {
            _spawnPositions = spawnPositions;
            _carSettings = carSettings;
            _createdCarData = carData;
            _pool = pool;
        }

        public void Initialize()
        {
            var spawnPoint = _spawnPositions.Skip(UnityEngine.Random.Range(0, _spawnPositions.Count())).FirstOrDefault();
            _currentCar = _pool.CreateCar<CarModel>(_carSettings.CarPrefab);
            _currentCar.Transform.position = spawnPoint.position;
            _currentCar.Transform.rotation = spawnPoint.rotation;
            _createdCarData.SetValue(_currentCar);
            _currentCar.FirstStarting(_carSettings, _createdCarData);
            UpdateManager.SubscribeToUpdate(Execute);
            UpdateManager.SubscribeToFixedUpdate(FastExecute);
        }

        public void Execute()
        {
            _currentCar.Ride();
        }

        public void FastExecute()
        {
            _currentCar.FixedRide();
        }

        public void Dispose()
        {
            UpdateManager.UnsubscribeFromUpdate(Execute);
            UpdateManager.UnsubscribeFromFixedUpdate(FastExecute);
        }
    }
}