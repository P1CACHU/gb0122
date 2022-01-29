using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CatalogItemsElement : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Text itemName;
    [SerializeField] private Text price;

    private CatalogItem _item;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    
    public void SetItem(CatalogItem item)
    {
        _item = item;
        itemName.text = _item.DisplayName;
        price.text = _item.VirtualCurrencyPrices.ToString();
    }

    private void OnClick()
    {
        if (!_item.VirtualCurrencyPrices.ContainsKey("HC"))
            return;

        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest {
            CatalogVersion = _item.CatalogVersion,
            ItemId = _item.ItemId,
            Price = (int)_item.VirtualCurrencyPrices["HC"],
            VirtualCurrency = "HC"
        }, success =>
        {
            Debug.LogError($"Item successfully bought: {success.Items.Count}");
        }, error =>
        {
            Debug.LogError($"Get User Inventory Failed: {error}");
        });
    }
}
