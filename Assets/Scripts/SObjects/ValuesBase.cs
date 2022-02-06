using System;
using System.Collections;
using UnityEngine;


namespace ExampleGB
{
    public abstract class ValuesBase<T> : ScriptableObject
    {
        private T _currentValue;

        public T Value => _currentValue;

        public Action<T> OnSelected;

        public void SetValue(T item)
        {
            _currentValue = item;
            OnSelected?.Invoke(_currentValue);
        }
    }
}