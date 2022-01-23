using System;
using UnityEngine;
using UnityEngine.UI;


namespace ExampleGB
{
    public class AccountEntranceSwitcher : MonoBehaviour
    {
        [SerializeField] private Button _signUpButton;
        [SerializeField] private Button _signInButton;
        [SerializeField] private AccountMenu _createAccountMenu;
        [SerializeField] private AccountMenu _logIntoAccountMenu;

    }
}