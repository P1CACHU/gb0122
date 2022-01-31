using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;


namespace ExampleGB
{
    public class RoomPanel : MonoBehaviour, IUiElement
    {
        private List<RoomElement> _roomElements;
        private int increment;

        public void Initialize()
        {
            increment = 0;
            _roomElements = new List<RoomElement>();

            if (_roomElements.Count == 0) _roomElements.AddRange(GetComponentsInChildren<RoomElement>());

            foreach (var element in _roomElements)
            {
                element.Initialize();
            }

            Close();
        }

        public void Close()
        {
            foreach (var element in _roomElements)
            {
                element.Close();
            }
        }        

        public void Show()
        {
            increment = 0;

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                _roomElements[increment].Show(player.NickName);
                increment++;
            }
        }

        public void ShowRoom(List<RoomInfo> roomList)
        {
            increment = 0;

            foreach(var room in roomList)
            {
                _roomElements[increment].Show(room.Name);
                increment++;
            }
        }
    }
}