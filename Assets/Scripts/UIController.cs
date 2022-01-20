using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshProUGUI playFabConnectionLabel;

    [SerializeField]
    private Button photonButton;

    [SerializeField]
    private TextMeshProUGUI photonButtonText;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }




    public void ConnectToPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            //  PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.Disconnect();
            photonButton.GetComponent<Image>().color = Color.red;
            photonButtonText.text = "Disconnected";
        }
        else
        {
            photonButton.GetComponent<Image>().color = Color.white;
            photonButtonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon Success");
        photonButton.GetComponent<Image>().color = Color.green;
        photonButtonText.text = "Connected";
    }

   

    public void ConnectToPlayFab()
    {
    
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A823B";
            Debug.Log("Title ID was installed");
        }
        PlayFabSettings.staticSettings.TitleId = "A823B";
        var request = new LoginWithCustomIDRequest { CustomId = "lesson3", CreateAccount = true };
        playFabConnectionLabel.text = "Connecting...";
        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnPlayFabLoginFailure);
    }


    public void BadConnectToPlayFab()
    {


            PlayFabSettings.staticSettings.TitleId = "XXXXX";
            Debug.Log("BAD Title ID was installed");
       
        var request = new LoginWithCustomIDRequest { CustomId = "lesson3", CreateAccount = true };
        playFabConnectionLabel.text = "Connecting...";
        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnPlayFabLoginFailure);
    }

    private void OnPlayFabLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab Success");
        playFabConnectionLabel.text = "Connected";
        playFabConnectionLabel.color = Color.green;

    }

    private void OnPlayFabLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Fail: {error}");
        playFabConnectionLabel.color = Color.red;
        playFabConnectionLabel.text = "Connection failed";
    }
}
