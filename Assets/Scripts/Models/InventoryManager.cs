using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;


namespace ExampleGB
{
    public sealed class InventoryManager : MonoBehaviour
    {
        [SerializeField] CatalogItemsElement _element;

        private void Start()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
            {
                HandleInventory(result.Inventory);
            }, Debug.LogError);
        }

        private void HandleInventory(List<ItemInstance> items)
        {
            foreach (var item in items)
            {
                //передать предмет в канвас инвентаря
            }
        }
    }
}