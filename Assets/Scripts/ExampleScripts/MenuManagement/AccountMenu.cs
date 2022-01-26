using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;


namespace ExampleGB
{
    public sealed class AccountMenu : MonoBehaviour, IMenuUI
    {
        public event Action<AccountInfo> OnSendingAccountInfo;

        [SerializeField] private TMP_InputField _username;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _email;

        [SerializeField] private Button _goNext;
        [SerializeField] private Button _close;

        private AccountManager _manager;

        public void Initialize(AccountManager manager)
        {
            _manager = manager;
            Initialize();
        }

        public void Initialize()
        {
            _close.onClick.AddListener(() => CloseMenu());
            _goNext.onClick.AddListener(() => GetAccountInfo());            
        }        

        public void ShowMenu()
        {
            gameObject.SetActive(true);
            _username.ActivateInputField();
        }

        public void CloseMenu()
        {
            _username.text = "";
            _password.text = "";
            if (_email != null) _email.text = "";
            gameObject.SetActive(false);
            _manager.OnCloseEvent?.Invoke();
        }

        private void GetAccountInfo()
        {
            if (_email != null)
            {
                OnSendingAccountInfo?.Invoke(new AccountInfo(LogInType.CreateAccount, _username.name, _password.name, _email.name));
            }
            else
            {
                OnSendingAccountInfo?.Invoke(new AccountInfo(LogInType.LogIn, _username.name, _password.name));
            }

            CloseMenu();
        }
    }
}