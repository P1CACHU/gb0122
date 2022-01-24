using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private ConnectionButtonWidget _connectButton;

    private void Awake()
    {
        _connectButton.Refresh(ConnectionState.Default, "Playfab connection button");

        _connectButton.button.onClick.AddListener(OnConnectButtonClick);
    }

    private void OnDestroy()
    {
        _connectButton.button.onClick.RemoveListener(OnConnectButtonClick);
    }

    void OnConnectButtonClick()
    {
        _connectButton.Refresh(ConnectionState.Waiting, "Waiting for connection");

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "9E620";
            Debug.Log("Title ID was installed");
        }

        var request = new LoginWithCustomIDRequest {CustomId = "lesson3", CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string playfabSuccessText = "PlayFab Success";
        
        Debug.Log(playfabSuccessText);
        _connectButton.Refresh(ConnectionState.Success, playfabSuccessText);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        string playfabFailText = $"Fail: {error}";

        Debug.Log(playfabFailText);
        _connectButton.Refresh(ConnectionState.Success, playfabFailText);
    }
}
