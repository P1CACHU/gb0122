using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace ExampleGB
{
    public sealed class PopUpMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _username;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _email;

        public AccountInfo GetAccountInfo()
        {
            return new AccountInfo(_username.name, _password.name, _email.name);
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
            _email.text = "";
            gameObject.SetActive(false);
        }
    }
}