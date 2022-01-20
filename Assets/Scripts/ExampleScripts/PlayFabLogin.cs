using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace ExampleGB
{
    public class PlayFabLogin : MonoBehaviour
    {
        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "1DD8D";
                Debug.Log("Title ID was installed");
            }

            var request = new LoginWithCustomIDRequest { CustomId = "lesson3", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("PlayFab Success");
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError($"Fail: {error}");
        }
    }
}
