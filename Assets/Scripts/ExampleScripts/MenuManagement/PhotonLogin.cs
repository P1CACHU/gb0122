using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace ExampleGB
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        public delegate void OnMessageRecieve(string message);
        public event OnMessageRecieve OnRecieveMSG;

        private const string CONNECTION = "Photon connection Success";
        private const string DISCONNECTION = "Photon disconnection";        

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            OnRecieveMSG?.Invoke(CONNECTION);
            Debug.Log(CONNECTION);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            OnRecieveMSG?.Invoke(DISCONNECTION);
            Debug.Log(DISCONNECTION);
        }
    }
}
