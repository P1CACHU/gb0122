using System;
using UnityEngine;
using UnityEngine.UI;


namespace ExampleGB
{
    public class AccountManager : MonoBehaviour, IMenuUI
    {
        public Action OnCloseEvent;

        [SerializeField] private Button _createAccountButton;
        [SerializeField] private Button _logInButton;
        [SerializeField] private Button _close;
        [SerializeField] private AccountMenu _createAccountMenu;
        [SerializeField] private AccountMenu _logIntoAccountMenu;

        public void Initialize()
        {            
            _createAccountMenu.Initialize(this);
            _logIntoAccountMenu.Initialize(this);
            _close.onClick.AddListener(() => CloseMenu());            
            _createAccountButton.onClick.AddListener(() => GoForward(LogInType.CreateAccount));
            _logInButton.onClick.AddListener(() => GoForward(LogInType.LogIn));
        }

        public void SubscribeOnRecieveInfo(Action<AccountInfo> callback)
        {
            _createAccountMenu.OnSendingAccountInfo += callback;
            _logIntoAccountMenu.OnSendingAccountInfo += callback;            
        }

        public void UnSubscribeRecieveInfo(Action<AccountInfo> callback)
        {
            _createAccountMenu.OnSendingAccountInfo -= callback;
            _logIntoAccountMenu.OnSendingAccountInfo -= callback;
        }

        public void GoForward(LogInType type)
        {
            switch (type)
            {
                case LogInType.CreateAccount:
                    _createAccountMenu.ShowMenu();
                    break;
                case LogInType.LogIn:
                    _logIntoAccountMenu.ShowMenu();
                    break;
            }

            DisableButtons();
        }

        public void CloseMenu()
        {
            DisableButtons();
            OnCloseEvent?.Invoke();
        }

        public void ShowMenu()
        {
            gameObject.SetActive(true);
            _close.interactable = true;
            _createAccountButton.interactable = true;
            _logInButton.interactable = true;
        }

        private void DisableButtons()
        {
            gameObject.SetActive(false);
            _close.interactable = false;
            _createAccountButton.interactable = false;
            _logInButton.interactable = false;
        }
    }
}