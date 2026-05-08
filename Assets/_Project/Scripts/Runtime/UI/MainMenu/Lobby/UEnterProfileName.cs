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

        SessionProvider sessionProvider;

        string username;
        string savedUsername;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            sessionProvider = SessionProvider.GetInstance;

            sessionProvider.OnSessionAdded += OnStartedSession;
            sessionProvider.OnSessionLeaved += OnSessionLeaved;

            username = sessionProvider.GetCurrentUsername();   
            savedUsername = username;

            inputField.text = username;

            inputField.onValueChanged.AddListener(UpdateField);
            updateButton.onClick.AddListener(OnUpdateButton);

            OnSessionLeaved();
        }

        void OnDestroy()
        {
            sessionProvider.OnSessionAdded -= OnStartedSession;
            sessionProvider.OnSessionLeaved -= OnSessionLeaved;
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
            
            await sessionProvider.UpdateUsername(inputField.text);

            inputField.interactable = true;
        }

        void OnStartedSession(PurrLobby.Lobby lobby)
        {
            OnStartedSession();
        }

        void OnStartedSession()
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