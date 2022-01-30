using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
    [SerializeField] private CatalogItemsElement _element;

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleCatalog(result.Catalog);
        //Debug.Log($"Catalog was loaded successfully!");
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            //Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            var element = Instantiate(_element, _element.transform.parent);
            element.gameObject.SetActive(true);
            element.SetItem(item);
        }
    }
}
