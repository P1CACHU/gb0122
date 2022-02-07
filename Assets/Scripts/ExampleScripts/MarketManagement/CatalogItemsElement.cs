using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ExampleGB
{
    public class CatalogItemsElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _price;

        private StoreItem _item;
        //переписать setItem компактнее

        public void SetItem(CatalogItem item)
        {
            _itemName.text = item.DisplayName;

            if (item.VirtualCurrencyPrices.ContainsKey("GD"))
            {
                _price.text = item.VirtualCurrencyPrices["GD"].ToString();
            }
        }

        public void SetItem(StoreItem item)
        {
            _itemName.text = item.ItemId;

            if (item.VirtualCurrencyPrices.ContainsKey("GD"))
            {
                _price.text = item.VirtualCurrencyPrices["GD"].ToString();
            }
        }

        public void SetItem(ItemInstance item)
        {
            _itemName.text = item.DisplayName;
        }

        public void MakePurchase()
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = "sw_cnt_key",
                Price = 10,
                VirtualCurrency = "GD"
            }, result => { }, Debug.LogError);

            //повесить кнопку на иконку в магазине и назначить на нее этот метод
        }
    }
}