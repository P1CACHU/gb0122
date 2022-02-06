using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class JoinRandomRoomButtonWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
