using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConnectionButtonWidget : ButtonWidget
{
    [SerializeField] private Text _text;
    [SerializeField] private ConnectionIndicator _connectionIndicator;

    private Dictionary<Type, UnityAction> _actions;

    protected override void Awake()
    {
        base.Awake();

        _text.text = String.Empty;
    }

    public void Refresh(ConnectionState resultState, string text)
    {
        RefreshIndicator(resultState);
        RefreshText(text);
    }

    public void Refresh(ConnectionState resultState)
    {
        RefreshIndicator(resultState);
    }

    void RefreshIndicator(ConnectionState resultState)
    {
        switch (resultState)
        {
            case ConnectionState.Waiting:
                _connectionIndicator.Refresh(resultState);
                break;

            case ConnectionState.Success:
                _connectionIndicator.Refresh(resultState);
                break;

            case ConnectionState.Fail:
                _connectionIndicator.Refresh(resultState);
                break;

            case ConnectionState.Default:
                _connectionIndicator.Refresh(resultState);
                break;

            default:
                throw new Exception("Unknown connection state");
        }
    }

    void RefreshText(string text)
    {
        _text.text = text;
    }
}
