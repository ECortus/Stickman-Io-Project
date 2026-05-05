using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UEnterProfileName : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button updateButton;

        ServicesInitializationProvider initializationProvider;
        SessionProvider sessionProvider;

        string username;
        string savedUsername;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            initializationProvider = ServicesInitializationProvider.GetInstance;
            sessionProvider = SessionProvider.GetInstance;

            sessionProvider.OnSessionAdded += (e) => OnSessionAdded();
            sessionProvider.OnSessionLeaved += OnSessionLeaved;

            username = initializationProvider.GetCurrentUsername();   
            savedUsername = username;

            inputField.text = username;

            inputField.onValueChanged.AddListener(UpdateField);
            updateButton.onClick.AddListener(OnUpdateButton);

            OnSessionLeaved();
        }

        void UpdateField(string value = "")
        {
            if (value != null && value.Length > 0)
            {
                username = value;
                if (username != savedUsername)
                {
                    updateButton.interactable = true;
                }
                else
                {
                    updateButton.interactable = false;
                }
            }
            else
            {
                updateButton.interactable = false;
            }
        }

        async void OnUpdateButton()
        {
            savedUsername = username;

            inputField.interactable = false;
            updateButton.interactable = false;
            
            await initializationProvider.UpdateUsername(inputField.text);

            inputField.interactable = true;
        }

        void OnSessionAdded()
        {
            inputField.interactable = false;
            updateButton.interactable = false;
        }

        void OnSessionLeaved()
        {
            inputField.interactable = true;
            UpdateField(username);
        }
    }
}