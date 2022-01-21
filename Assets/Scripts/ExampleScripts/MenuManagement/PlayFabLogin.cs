using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace ExampleGB
{
    public class PlayFabLogin
    {
        public delegate void OnMessageRecieve(string message);
        public event OnMessageRecieve OnRecieveMSG;

        private const string TITLE_ID = "Title ID was installed";
        private const string CONNECTION = "PlayFab Success";

        private string _customId;

        public void CreateCustomId(string name)
        {
            _customId = name;
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "1DD8D";
                OnRecieveMSG?.Invoke(TITLE_ID);
                Debug.Log(TITLE_ID);
            }

            var request = new LoginWithCustomIDRequest { CustomId = _customId, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
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
