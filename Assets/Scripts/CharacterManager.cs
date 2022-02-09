using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private const string CharacterStoreId = "st_2";

    [SerializeField] private InputField _inputField;
    [SerializeField] private Button _buyCharacterButton;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _startText;
    [SerializeField] private Text _firstUserCharacterDataText;

    private void Awake()
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "HC",
            Amount = 100
        }, result => {Debug.Log("Added hard currency");}, Debug.LogError);

        _buyCharacterButton.onClick.AddListener(OnClick);
    }
    
    private void OnDestroy()
    {
        _buyCharacterButton.onClick.RemoveListener(OnClick);
    }

    private void Start()
    {
        GetAllUsersCharacters();
    }

    private void SetUserCharactersCount(int userCharactersCount)
    {
        _startText.text = $"You have {userCharactersCount} characters";
    }

    private void GetAllUsersCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), 
            result =>
            {
                SetUserCharactersCount(result.Characters.Count);
                if (result.Characters.Count == 0)
                    return;

                CharacterResult character = result.Characters[0];
                _firstUserCharacterDataText.text = $"The first one is {character.CharacterName} of type {character.CharacterType}";
            },
            Debug.LogError);
    }

    private void OnClick()
    {
        var characterName = _inputField.text;

        if (string.IsNullOrEmpty(characterName))
        {
            _resultText.text = "Write character name";
            return;
        }
        
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest { StoreId = CharacterStoreId }, 
            result => PurchaseItem(result.Store),
            Debug.LogError);
    }

    private void PurchaseItem(List<StoreItem> items)
    {
        if (items.Count == 0)
        {
            _resultText.text = "Store has no characters";
            return;
        }

        StoreItem item = items[0];
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            ItemId = item.ItemId,
            Price = (int)item.VirtualCurrencyPrices["HC"],
            VirtualCurrency = "HC"
        },
        result => GrantCharacterToUser(result.Items[0].ItemId),
        Debug.LogError);
    }

    private void GrantCharacterToUser(string characterId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            ItemId = characterId,
            CharacterName = _inputField.text
        },
            result => UpdateCharacterStatistics(result.CharacterId),
            Debug.LogError);
    }
    
    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest()
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {"Damage", 2},
                    {"Health", 3},
                    {"Experience", 1}
                }
            },
            result => GetAllUsersCharacters(),
            Debug.LogError);
    }
}
