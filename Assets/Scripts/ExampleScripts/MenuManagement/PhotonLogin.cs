using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace ExampleGB
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        public delegate void OnMessageRecieve(string message);
        public event OnMessageRecieve OnRecieveMSG;

        private const string CONNECTION = "Photon connection Success";
        private const string DISCONNECTION = "Photon disconnection";

        [SerializeField] private TMP_InputField _roomName;
        [SerializeField] private TMP_InputField _maxPlayers;

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

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate Success");
        }

        public void OnCreateRoomButtonClicked()
        {
            string roomName = _roomName.text;
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

            byte maxPlayers;
            byte.TryParse(_maxPlayers.text, out maxPlayers);
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

            PhotonNetwork.CreateRoom(roomName, options);
        }
    }
}
