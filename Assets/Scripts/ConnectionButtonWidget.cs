using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ConnectionState
{
    Default,
    Success,
    Fail,
    Waiting
}

public class ConnectionButtonWidget : MonoBehaviour
{
    public Button button => _button;

    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;

    [Header("Button state colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Color _waitingColor;

    public void Refresh(ConnectionState resultState, string text)
    {
        switch (resultState)
        {
            case ConnectionState.Waiting:
                _button.image.color = _waitingColor;
                break;

            case ConnectionState.Success:
                _button.image.color = _successColor;
                break;

            case ConnectionState.Fail:
                _button.image.color = _failColor;
                break;

            case ConnectionState.Default:
                _button.image.color = _defaultColor;
                break;

            default:
                throw new Exception("Unknown connection state");
        }
        
        _text.text = text;
    }
}
