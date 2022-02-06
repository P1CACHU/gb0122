using System;
using UnityEngine;


namespace ExampleGB
{
    [CreateAssetMenu(fileName = nameof(SelectedCarData), menuName = "SObject/" + nameof(SelectedCarData))]
    public class SelectedCarData : ValuesBase<CarModel>
    {
        private float _throttleInput;
        private float _steerInput;

        public Action<float, float> OnThrottleAndSteer;

        public void SetInputValues(float throttle, float steer)
        {
            _throttleInput = throttle;
            _steerInput = steer;
            OnThrottleAndSteer?.Invoke(_throttleInput, _steerInput);
        }
    }
}