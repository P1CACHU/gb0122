using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


namespace ExampleGB
{
    [RequireComponent(typeof(PhotonLogin))]
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private Button _startPhotonButton;
        [SerializeField] private Button _shutDownPhotonButton;
        [SerializeField] private Button _connectPlayfabButton;
        [SerializeField] private Button _closeApplication;

        [SerializeField] private AccountManager _accountManager;
        [SerializeField] private ChatFieldUI _textField;

        private PhotonLogin _photon;
        private PlayFabLogin _playFabLogin;

        private void Awake()
        {
            _photon = GetComponent<PhotonLogin>();
            if (_playFabLogin == null) _playFabLogin = new PlayFabLogin();
        }

        private void Start()
        {
            _textField.Initialize();
            _accountManager.Initialize();
            _playFabLogin.Connect();            
            _startPhotonButton.onClick.AddListener(() => StartPhoton());
            _shutDownPhotonButton.onClick.AddListener(() => ShutDownPhoton());
            _connectPlayfabButton.onClick.AddListener(() => StartPlayfab());
            _closeApplication.onClick.AddListener(() => Close());
            _photon.OnRecieveMSG += RecieveMessage;
            _playFabLogin.OnRecieveMSG += RecieveMessage;
            _accountManager.OnCloseEvent += EnableButtons;
        }

        private void StartPhoton()
        {
            _photon.Connect();
            _startPhotonButton.interactable = false;
            _shutDownPhotonButton.interactable = true;
            PlayerWelcome();
        }

        private void ShutDownPhoton()
        {
            _photon.Disconnect();
            _startPhotonButton.interactable = true;
            _shutDownPhotonButton.interactable = false;
        }

        private void StartPlayfab()
        {
            DisableButtons();
            _accountManager.SubscribeOnRecieveInfo(GetAccountInformation);
            _accountManager.ShowMenu();   
        }

        private void GetAccountInformation(AccountInfo info)
        {
            switch (info.LogIn)
            {
                case LogInType.CreateAccount:
                    _playFabLogin.CreateAccount(info);
                    break;
                case LogInType.LogIn:
                    _playFabLogin.LogIntoAccount(info);
                    break;
            }
        }

        private void PlayerWelcome()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
            {
                RecieveMessage($"Welcome back, Player {success.AccountInfo.PlayFabId}");
                RecieveMessage($"Created {success.AccountInfo.Created.Date}");
            }, errorCallback =>
            {
            });
        }

        private void RecieveMessage(string message)
        {
            _textField.RecieveMessage(message);
        }

        private void EnableButtons()
        {
            _startPhotonButton.interactable = true;
            _connectPlayfabButton.interactable = true;
            _accountManager.UnSubscribeRecieveInfo(GetAccountInformation);
        }

        private void DisableButtons()
        {
            _startPhotonButton.interactable = false;
            _shutDownPhotonButton.interactable = false;
            _connectPlayfabButton.interactable = false;
        }

        private void Close()
        {
            _photon.OnRecieveMSG -= RecieveMessage;
            _playFabLogin.OnRecieveMSG -= RecieveMessage;
            _accountManager.OnCloseEvent -= EnableButtons;
            Application.Quit();
        }
    }
}