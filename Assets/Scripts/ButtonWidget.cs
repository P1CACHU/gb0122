using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private Dictionary<Type, UnityAction> _actions;
    
    protected virtual void Awake()
    {
        _actions = new Dictionary<Type, UnityAction>();

        _button.onClick.AddListener(OnButtonClick);
    }

    protected virtual void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void AddListener(Type type, UnityAction action)
    {
        _actions.Add(type, action);
    }
    
    public void RemoveListener(Type type)
    {
        _actions.Remove(type);
    }

    void OnButtonClick()
    {
        foreach (UnityAction action in _actions.Values)
            action.Invoke();
    }
}