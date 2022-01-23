using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace ExampleGB
{
    public sealed class AccountMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _username;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _email;

        [SerializeField] private Button _goNext;
        [SerializeField] private Button _close;

        public Button GoNext => _goNext;
        public Button Close => _close;

        public void Initialize()
        {
            _close.onClick.AddListener(() => CloseMenu());
        }

        public AccountInfo GetAccountInfo(bool signIn = false)
        {
            return (signIn == false) ? new AccountInfo(_username.name, _password.name, _email.name) :
                new AccountInfo(_username.name, _password.name);
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
        }
    }
}