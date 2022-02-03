using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogItemsElement : MonoBehaviour
{
    [SerializeField] private Text ItemName;
    [SerializeField] private Text ItemPrice;
    [SerializeField] private Text CurrentXP;
    public void SetItem(CatalogItem item)
    {
        ItemName.text = item.DisplayName;
        ItemPrice.text = item.VirtualCurrencyPrices["XP"].ToString();
    }

    public void BuyItem()
    {
        if (int.Parse(CurrentXP.text) > int.Parse(ItemPrice.text))
        {
            Debug.Log($"Buy the {ItemName.text}({ItemPrice.text}XP) is success");

            PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
            {
                VirtualCurrency = "XP",
                Amount = int.Parse(ItemPrice.text)
            }
            , success =>
            {
                Debug.Log($"Покупка: -{ItemPrice.text}");
                UpdateXP();
            }, errorCallback =>
            {
                Debug.Log($"Error SubtractUserVirtualCurrency:{errorCallback}");
            });
        }
        else
            Debug.Log("not enough XP");
    }
    public void UpdateXP()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
        , success =>
        {
            Debug.Log("Значение кошелька загружено");
            CurrentXP.text = success.VirtualCurrency["XP"].ToString();
        }, errorCallback =>
        {
            Debug.Log($"Error GetCharacterInventory{errorCallback}");
        });
    }
}
