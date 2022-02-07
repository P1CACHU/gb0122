using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace ExampleGB
{
    public class MainSceneController : MonoBehaviour, IDisposable
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private PopUpMenu _popUpMenu;

        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
        private List<IMenuObject> _menus;
        private BaseMenuObject _currentMenu;

        private static event Action OnUpdateEvent;
        private static bool _isPaused = false;

        private void Awake()
        {
            _menus = new List<IMenuObject>();
        }

        private void Start()
        {
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                StoreId = "items_store"
            }, result =>
            {
                OnGetItemsSuccess(result.Store);
            }, OnFailure);

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);

            _menus.Add(_mainMenu);
            _menus.Add(_shopMenu);
            _menus.Add(_popUpMenu);

            foreach (var menu in _menus)
            {
                menu.Hide();
            }

            _mainMenu.OpenShopMenu.onClick.AddListener(delegate { ShowMenu(InterfaceObject.ShopMenu); });
            _shopMenu.CloseMenu.onClick.AddListener(delegate { ShowMenu(InterfaceObject.MainMenu); });

            if (_mainMenu != null)
            {
                ShowMenu(InterfaceObject.MainMenu);
            }
        }

        private void Update()
        {
            if (OnUpdateEvent != null && !_isPaused) OnUpdateEvent.Invoke();
        }

        public void ShowMenu(InterfaceObject menuObject)
        {
            if (_currentMenu != null) _currentMenu.Hide();

            switch (menuObject)
            {
                case InterfaceObject.MainMenu:
                    _currentMenu = _mainMenu;
                    _currentMenu.Show();
                    break;
                case InterfaceObject.ShopMenu:
                    _currentMenu = _shopMenu;
                    _shopMenu.Activate();
                    _currentMenu.Show();
                    break;
                default:
                    break;
            }
        }

        public static void Pause()
        {
            _isPaused = !_isPaused;
            Time.timeScale = (_isPaused == true) ? 0 : 1;
        }

        public static void SubscribeToUpdate(Action callback)
        {
            OnUpdateEvent += callback;
        }

        public static void UnsubscribeFromUpdate(Action callback)
        {
            OnUpdateEvent -= callback;
        }

        public void Dispose()
        {
            OnUpdateEvent = null;
            _currentMenu = null;
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.Log($"Something wrong in catalogs: {errorMessage}");
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            Debug.Log($"Catalog loaded successfully!");
        }

        private void OnGetItemsSuccess(List<StoreItem> store)
        {
            foreach (var item in store)
            {
                //заполнить инвентарь
            }
        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach(var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
                Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            }
        }
    }
}