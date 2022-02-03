using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    private string _roomName;

    //[SerializeField] private GameObject playerList;
    //[SerializeField] private GameObject createRoomPanel;
    //[SerializeField] private PlayersElement element;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;
    public GameObject PanelRoom;
    public GameObject PanelPlayers;
    public GameObject PlayerItemPrefab;
    public Button BtnCloseRoom;
    public Button BtnFriends;
    public bool FriendsRoom=false;
    //public Button button;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        Connect();
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

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        //PhotonNetwork.JoinRandomRoom();
        Debug.Log("Photon Success");
        PhotonNetwork.JoinLobby();
    }


    public void UpdateRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed {message}");
    }

    public void OnCreateRoomButtonClicked()
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 10 };
        options.PublishUserId = true;
        options.IsVisible = !FriendsRoom;
        PhotonNetwork.CreateRoom(_roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("On Joined room success!");
        PanelRoom.SetActive(true);
        PanelRoom.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
        BtnCloseRoom.image.color = Color.green;

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerItemPrefab);
            entry.transform.SetParent(PanelPlayers.transform);
            entry.GetComponent<PlayerItem>().Initialize(p.UserId, p.NickName);

            playerListEntries.Add(p.ActorNumber, entry);
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("On Joined lobby success!");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate!");
        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public void BtnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("LeaveRoom");
    }
    public void BtnJoinRoomByName()
    {
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.IsOpen)
            BtnCloseRoom.image.color = Color.green;
        else
            BtnCloseRoom.image.color = Color.red;
    }
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("UpdateCachedRoomList");
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }
    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.GetComponent<ListRooms>().SetItemRoom(info);

            roomListEntries.Add(info.Name, entry);
        }
    }


    public override void OnLeftRoom()
    {
        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        playerListEntries.Clear();
        playerListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerItemPrefab);
        entry.transform.SetParent(PanelPlayers.transform);
        entry.GetComponent<PlayerItem>().Initialize(newPlayer.UserId, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }

    public void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = !PhotonNetwork.CurrentRoom.IsOpen;
        if (PhotonNetwork.CurrentRoom.IsOpen)
            BtnCloseRoom.image.color = Color.green;
        else
            BtnCloseRoom.image.color = Color.red;
    }
    public void BtnFriendsRoom()
    {
        FriendsRoom = !FriendsRoom;
        if (FriendsRoom)
            BtnFriends.image.color = Color.green;
        else
            BtnFriends.image.color = Color.white;
    }
}