using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject[] _interactiveObjects;
    
    private string _inputText;

    private void Start()
    {
        Array.ForEach(_interactiveObjects, go => go.SetActive(false));

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), success =>
        {
            nickname.text = $"Player Name: {success.PlayerProfile.DisplayName}";

            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = success.PlayerProfile.PlayerId,
                Keys = null
            }, result =>
            {
                var userHealth = int.Parse(result.Data["Health"].Value);
                var playerHealth = Resources.Load<PlayerHealth>(nameof(PlayerHealth));
                playerHealth.health = userHealth;
                Array.ForEach(_interactiveObjects, go => go.SetActive(userHealth > 0));
            }, error =>
            {
                Debug.Log("Got error retrieving user data:");
            });
        }, Debug.LogError);
    }

    public void UpdateInputFiled(string text)
    {
        _inputText = text;
    }

    public void SaveNicknameOnPlayFab()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _inputText
        }, result => { }, Debug.LogError);
    }
    
    public void ClearCredentials()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Bootstrap");
    }
}
