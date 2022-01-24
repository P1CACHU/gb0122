using System;
using UnityEngine;
using UnityEngine.UI;

public enum ConnectionState
{
    Default,
    Success,
    Fail,
    Waiting
}

public class ConnectionIndicator : MonoBehaviour
{
    [SerializeField] private Image _indicationImage;
    
    [Header("State colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Color _waitingColor;

    public void Refresh(ConnectionState resultState)
    {
        switch (resultState)
        {
            case ConnectionState.Waiting:
                _indicationImage.color = _waitingColor;
                break;

            case ConnectionState.Success:
                _indicationImage.color = _successColor;
                break;

            case ConnectionState.Fail:
                _indicationImage.color = _failColor;
                break;

            case ConnectionState.Default:
                _indicationImage.color = _defaultColor;
                break;

            default:
                throw new Exception("Unknown connection state");
        }
    }
}