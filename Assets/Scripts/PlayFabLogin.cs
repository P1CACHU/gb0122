using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    public const string AuthKey = "player-unique-id";

    [SerializeField] private ConnectionButtonWidget _connectButton;
    [SerializeField] private PlayerInfo _playerInfo;

    private void Start()
    {
        _connectButton.AddListener(GetType(), LoginWithCustomID);
    }

    private void OnDestroy()
    {
        _connectButton.RemoveListener(GetType());
    }

    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _playerInfo.username,
            Email = _playerInfo.mail,
            Password = _playerInfo.pass,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log("Success");
        }, errorCallback => {
            Debug.LogError($"Error: {errorCallback}");
        });
    }

    public void Login()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _playerInfo.username,
            Password = _playerInfo.pass
        }, result =>
        {
            Debug.Log($"Success: {_playerInfo.username}");
        }, errorCallback =>
        {
            Debug.LogError($"Error: {errorCallback}");
        });
    }

    private void LoginWithCustomID()
    {
        _connectButton.Refresh(ConnectionState.Waiting);

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "9E620";
            Debug.Log("Title ID was installed");
        }

        var needCreation = !PlayerPrefs.HasKey(AuthKey);
        Debug.Log($"needCreation = {needCreation}");
        var id = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());
        Debug.Log($"id = {id}");
        var request = new LoginWithCustomIDRequest {CustomId = id, CreateAccount = needCreation};
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            OnLoginSuccess(id);
        }, OnLoginFailure);
    }

    private void OnLoginSuccess(string id)
    {
        PlayerPrefs.SetString(AuthKey, id);
        SceneManager.LoadScene("MainProfile");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        string playfabFailText = $"Fail: {error}";

        Debug.Log(playfabFailText);
        _connectButton.Refresh(ConnectionState.Success, playfabFailText);
    }
}
