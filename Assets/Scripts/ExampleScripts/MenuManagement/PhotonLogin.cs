using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

        [SerializeField] private Button _createRoom;
        [SerializeField] private Button _joinRandomRoom;
        [SerializeField] private Button _startGame;

        [SerializeField] private RoomPanel _roomPanel;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Initialize()
        {
            _roomPanel.Initialize();
            _createRoom.onClick.AddListener(() => OnCreateRoomButtonClicked());
            _startGame.onClick.AddListener(() => OnStartGameButtonClicked());
            _joinRandomRoom.onClick.AddListener(() => JoinRandomRoom());
            _startGame.interactable = false;
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
            PhotonNetwork.JoinLobby(TypedLobby.Default);
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
            _roomPanel.ShowRoom(roomList);
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
            
            _roomName.text = "";
            _maxPlayers.text = "";
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();            
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log(message);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                Debug.Log($"User {player.UserId} joined to room");
                _roomPanel.Show();
            }


            //foreach (Player player in PhotonNetwork.PlayerList)
            //{
            //    Debug.Log($"User {player.UserId} joined to room");

            //    //var newElement = сослатьс€ на €чейку UI на панели
            //    //активировать €чейку
            //    //передать параментры в €чейку
            //    //убрать дебаг лог

            //    _roomPanel.Show(player.UserId);
            //}
        }

        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1); //todo
        }
    }
}
