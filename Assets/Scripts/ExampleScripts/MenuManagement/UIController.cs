using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ExampleGB
{
    [RequireComponent(typeof(PhotonLogin))]
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private Button _startPhotonButton;
        [SerializeField] private Button _shutDownPhotonButton;
        [SerializeField] private Button _connectPlayfabButton;
        [SerializeField] private Button _closeApplication;

        [SerializeField] private PopUpMenu _nameMenu;
        [SerializeField] private ChatFieldUI _textField;

        private PhotonLogin _photon;
        private PlayFabLogin _playFab;

        private void Awake()
        {
            _photon = GetComponent<PhotonLogin>();
            _playFab = new PlayFabLogin();
        }

        private void Start()
        {
            _textField.Initialize();
            _nameMenu.Initialize();
            _nameMenu.ShowMenu();
            DisableButtons();
            _startPhotonButton.onClick.AddListener(() => StartPhoton());
            _shutDownPhotonButton.onClick.AddListener(() => ShutDownPhoton());
            _connectPlayfabButton.onClick.AddListener(() => StartPlayfab());
            _closeApplication.onClick.AddListener(() => Close());
            _photon.OnRecieveMSG += RecieveMessage;
            _playFab.OnRecieveMSG += RecieveMessage;
        }

        private void Update()
        {
            if (_nameMenu.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Return))
            {
                TypeName();
            }
        }

        private void StartPhoton()
        {            
            _photon.Connect();
            _startPhotonButton.interactable = false;
            _shutDownPhotonButton.interactable = true;
        }

        private void ShutDownPhoton()
        {
            _photon.Disconnect();
            _startPhotonButton.interactable = true;
            _shutDownPhotonButton.interactable = false;
        }

        private void StartPlayfab()
        {            
            _playFab.Connect();
            _connectPlayfabButton.interactable = false;            
        }

        private void TypeName()
        {
            _playFab.CreateCustomId(_nameMenu.TypeName());
            _nameMenu.CloseMenu();
            EnableButtons();
        }

        private void RecieveMessage(string message)
        {
            _textField.RecieveMessage(message);
        }

        private void EnableButtons()
        {
            _startPhotonButton.interactable = true;
            _connectPlayfabButton.interactable = true;
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
            _playFab.OnRecieveMSG -= RecieveMessage;
            Application.Quit();
        }
    }
}