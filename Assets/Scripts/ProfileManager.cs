using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private Text _id;
    [SerializeField] private Text _customIdInfo;
    [SerializeField] private Text _firstLogin;
    [SerializeField] private Text _hours;
    [SerializeField] private Text _xp;
    [SerializeField] private GameObject WindowLoad;
    private const string AuthKey = "player-unique-id";
    // Start is called before the first frame update
    void Start()
    {
        WindowLoad.SetActive(true);
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
        {
            WindowLoad.SetActive(false);
            _id.text = $"{success.AccountInfo.PlayFabId}";
            _hours.text = $"{success.AccountInfo.Created.Hour}";
            _customIdInfo.text = $"{success.AccountInfo.CustomIdInfo.CustomId}";
        }, errorCallback =>
        {
            Debug.Log("Error");
            WindowLoad.SetActive(false);
        });

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowCreated = true,
                ShowLastLogin = true,
                ShowValuesToDate = true
            }
        }
            , success =>
        {
            _firstLogin.text = success.PlayerProfile.Created.ToString();
        }, errorCallback =>
        {
            Debug.Log($"Error GetPlayerProfile{errorCallback}");
        });

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
        , success =>
        {
            Debug.Log("Значение кошелька загружено");
            _xp.text = success.VirtualCurrency["XP"].ToString();
        }, errorCallback =>
        {
            Debug.Log($"Error GetCharacterInventory{errorCallback}");
        });

    }
    public void DeleteProfile()
    {
        PlayerPrefs.DeleteKey(AuthKey);
        Debug.Log($"Профиль удален");
        SceneManager.LoadScene("SampleScene");
    }
}


//PlayFabClientAPI.GetCharacterInventory(new GetCharacterInventoryRequest()
//{

//}
//, success =>
// {
//     Debug.Log("Значение кошелька загружено");
//     _xp.text = success.VirtualCurrency["XP"].ToString();
// },errorCallback =>
// {
//     Debug.Log($"Error GetCharacterInventory{errorCallback}");
// });

//PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
//{
//    ProfileConstraints = new PlayerProfileViewConstraints()
//    {
//        ShowDisplayName = true,
//        ShowCreated = true,
//        ShowLastLogin = true,
//        ShowValuesToDate = true
//    }
//}
//    , success =>
//{
//    Debug.Log("Значение кошелька загружено");
//    _xp.text = success.PlayerProfile.ValuesToDate[0].Currency;
//    //_xp.text = $"{success.PlayerProfile.DisplayName} {success.PlayerProfile.ContactEmailAddresses}" +
//    //$"{success.PlayerProfile.Created}{success.PlayerProfile.Locations}";
//}, errorCallback =>
//{
//    Debug.Log($"Error GetPlayerProfile{errorCallback}");
//});