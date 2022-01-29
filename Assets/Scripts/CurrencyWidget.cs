using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class CurrencyWidget : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private CurrencyElement _currencyElement;

    private void Start()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            success =>
            {
                foreach (KeyValuePair<string, int> kv in success.VirtualCurrency)
                {
                    var currencyElement = Instantiate(_currencyElement, _content);
                    currencyElement.SetCurrency(kv);
                }
            }, 
            error =>
            {
                Debug.LogError($"Get User Inventory Failed: {error}");
            });
    }
}