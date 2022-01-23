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

        private string _username;
        //private string _password;
        //private string _email;

        //public void UpdateInfo(AccountInfo info)
        //{
        //    _username = info.Username;
        //    _password = info.Password;
        //    _email = info.Email;
        //}

        public void CreateAccount(AccountInfo info)
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = info.Username,
                Email = info.Email,
                Password = info.Password,
                RequireBothUsernameAndEmail = true
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
                OnRecieveMSG?.Invoke(CREATE_ACCOUNT_SUCCESS + " " + info.Username);
                Debug.Log(CREATE_ACCOUNT_SUCCESS);
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

            var needCreation = PlayerPrefs.HasKey("player-unique-id");
            var id = Guid.NewGuid().ToString();
            var request = new LoginWithCustomIDRequest { CustomId = _username, CreateAccount = !needCreation };
            PlayFabClientAPI.LoginWithCustomID(request, result =>
            {
                PlayerPrefs.SetString("player-unique-id", id);
            }, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {            
            OnRecieveMSG?.Invoke(CONNECTION);
            Debug.Log(CONNECTION);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            OnRecieveMSG?.Invoke($"Fail: {error}");
            Debug.LogError($"Fail: {error}");
        }
    }
}
