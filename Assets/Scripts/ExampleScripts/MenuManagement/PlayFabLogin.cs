using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


namespace ExampleGB
{
    public class PlayFabLogin
    {
        public delegate void OnMessageRecieve(string message);
        public event OnMessageRecieve OnRecieveMSG;

        private const string TITLE_ID = "Title ID was installed";
        private const string CONNECTION = "PlayFab Success";
        private const string CREATE_ACCOUNT_SUCCESS = "Account creation Success";
        private const string LOGIN_SUCCESS = "Login Success";
        private const string AUTH_KEY = "player-unique-id";

        public void CreateAccount(AccountInfo info)
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = info.Username,
                //Email = info.Email,
                Password = info.Password,
                DisplayName = info.Username,
                RequireBothUsernameAndEmail = false
            }, resuil =>
            {
                OnRecieveMSG?.Invoke(CREATE_ACCOUNT_SUCCESS);
                Debug.Log(CREATE_ACCOUNT_SUCCESS);
            }, error =>
            {
                OnRecieveMSG?.Invoke($"Error: {error}");
                Debug.Log($"Error: {error}");
            });
        }

        public void LogIntoAccount(AccountInfo info)
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = info.Username,
                Password = info.Password,
            }, resuil =>
            {
                OnRecieveMSG?.Invoke(LOGIN_SUCCESS + " " + info.Username);
                Debug.Log(LOGIN_SUCCESS);
            }, error =>
            {
                OnRecieveMSG?.Invoke($"Error: {error}");
                Debug.Log($"Error: {error}");
            });
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "1DD8D";
                OnRecieveMSG?.Invoke(TITLE_ID);
                Debug.Log(TITLE_ID);
            }

            var needCreation = !PlayerPrefs.HasKey(AUTH_KEY);
            Debug.Log($"Need creation = {needCreation}");
            var id = PlayerPrefs.GetString(AUTH_KEY, Guid.NewGuid().ToString());
            Debug.Log($"Id = {id}");
            var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = needCreation };
            PlayFabClientAPI.LoginWithCustomID(request, result =>
            {
                PlayerPrefs.SetString(AUTH_KEY, id);
                OnRecieveMSG?.Invoke(CONNECTION);
            }, OnLoginFailure);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            OnRecieveMSG?.Invoke($"Fail: {error}");
            Debug.LogError($"Fail: {error}");
        }
    }
}
