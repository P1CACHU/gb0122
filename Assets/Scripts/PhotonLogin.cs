using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectionButtonWidget _connectButton;
    [SerializeField] private ConnectionButtonWidget _disconnectButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _connectButton.Refresh(ConnectionState.Default, "Connect to Photon button");
        _disconnectButton.Refresh(ConnectionState.Default, "Disconnect from Photon button");

        _connectButton.button.onClick.AddListener(OnConnectButtonClick);
        _disconnectButton.button.onClick.AddListener(OnDisconnectButtonClick);
    }

    private void OnDestroy()
    {
        _connectButton.button.onClick.RemoveListener(OnConnectButtonClick);
        _disconnectButton.button.onClick.RemoveListener(OnDisconnectButtonClick);
    }

    void OnConnectButtonClick()
    {
        _disconnectButton.Refresh(ConnectionState.Default, "Disconnect from Photon button");

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            _connectButton.Refresh(ConnectionState.Waiting, "Waiting for connection to Photon");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void OnDisconnectButtonClick()
    {
        _connectButton.Refresh(ConnectionState.Default, "Connect Photon button");

        if (PhotonNetwork.IsConnected)
        {
            _disconnectButton.Refresh(ConnectionState.Waiting, "Waiting for disconnection from Photon");
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        string photonSuccessText = "Successfuly connected to Photon";
        
        Debug.Log(photonSuccessText);
        _connectButton.Refresh(ConnectionState.Success, photonSuccessText);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        string photonSuccessText = "Successfuly disconnected from Photon";
        
        Debug.Log(photonSuccessText);
        _disconnectButton.Refresh(ConnectionState.Success, photonSuccessText);
    }
}
