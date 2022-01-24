using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerInfoGo;
    [SerializeField] private Text _playerInfoText;
    [SerializeField] private ButtonWidget _resetAccountButton;

    private void Start()
    {
        _playerInfoGo.SetActive(false);
        _resetAccountButton.AddListener(GetType(), ResetAccount);

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), SetAccountInfo, errorCallback =>
        {
            _playerInfoText.text = $"Something went wrong:\n {errorCallback}";
        });
    }

    private void OnDestroy()
    {
        _resetAccountButton.RemoveListener(GetType());
    }

    void SetAccountInfo(GetAccountInfoResult success)
    {
        _playerInfoGo.SetActive(true);
        _playerInfoText.text = $"Welcome back, Player {success.AccountInfo.PlayFabId}\n" +
                               $"You create account at {success.AccountInfo.Created}\n" +
                               $"Your username is {success.AccountInfo.Username}";
    }

    private void ResetAccount()
    {
        PlayerPrefs.DeleteKey(PlayFabLogin.AuthKey);
        SceneManager.LoadScene("Bootstrap");
    }
}
