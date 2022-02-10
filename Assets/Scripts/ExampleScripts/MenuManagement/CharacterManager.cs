using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;


namespace ExampleGB
{
    public class CharacterManager : MonoBehaviour
    {
        private const string _storeId = "characters_store";
        private const string _virtualCurrency = "GD";
        [SerializeField] private string _inputFieldText;

        private void Start()
        {
            UpdateCharacters();
        }

        public void OnNameChanged(string changedName)
        {
            _inputFieldText = changedName;
        }

        public void OnCreateButtonClicked()
        {
            if (string.IsNullOrEmpty(_inputFieldText))
            {
                Debug.Log($"input field should not be empty");
                return;
            }

            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                StoreId = _storeId
            }, result => 
            {
                HandleStoreResult(result.Store);
            }, Debug.LogError);
        }

        private void HandleStoreResult(List<StoreItem> items)
        {
            foreach (var item in items)
            {
                Debug.Log($"{item.ItemId}");
                PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
                {
                    ItemId = item.ItemId,
                    Price = (int)item.VirtualCurrencyPrices[_virtualCurrency],
                    VirtualCurrency = _virtualCurrency
                }, result =>
                {
                    Debug.Log($"Item {result.Items[0].ItemId} was purchased");
                    TransformItemIntoCharacter(result.Items[0].ItemId);
                }, Debug.LogError);
            }
        }

        private void TransformItemIntoCharacter(string itemId)
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                ItemId = itemId,
                CharacterName = _inputFieldText
            }, result =>
            {
                UpdateCharacterStatistics(result.CharacterId);
            }, Debug.LogError);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    { "Level", 0},
                    { "Exp", 0},
                    { "Health", 100}
                }
            },result =>
            {
                // обновить UI
                //createPanel setActive false
                UpdateCharacters();
            }, Debug.LogError);
        }

        private void UpdateCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), result =>
            {
                for (int i = 0; i != 2 && i != result.Characters.Count; ++i)
                {
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = result.Characters[i].CharacterId
                    }, characterStatisticsResult =>
                    {
                        //сделать текстовые отображения персонажей
                        // name.txt = result.Characters[i].CharacterId
                        // lvl.text= result.CharacterStatistics("Level").ToString();
                    }, Debug.LogError);
                }
                //createPanel setActive true
                //разобраться с отображением панелей
            }, Debug.LogError);
        }
    }
}