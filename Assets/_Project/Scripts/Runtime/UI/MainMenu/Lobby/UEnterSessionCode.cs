using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UEnterSessionCode : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Button enterButton;

        SessionProvider provider;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;

            provider.OnAddingSessionStarted += OnStartedSession;
            provider.OnSessionAdded += OnStartedSession;
            provider.OnAddingSessionFailed += OnLeavedSession;
            provider.OnSessionLeaved += OnLeavedSession;

            inputField.onValueChanged.AddListener(OnUpdateField);
            enterButton.onClick.AddListener(OnEnterButtonClicked);

            OnLeavedSession();
            OnUpdateField();
        }

        void OnDestroy()
        {
            provider.OnAddingSessionStarted -= OnStartedSession;
            provider.OnSessionAdded -= OnStartedSession;
            provider.OnAddingSessionFailed -= OnLeavedSession;
            provider.OnSessionLeaved -= OnLeavedSession;
        }

        void OnUpdateField(string value = "")
        {
            if (value != null && value.Length > 0)
            {
                provider.SetSessionCode(value);
                enterButton.interactable = true;
            }
            else
            {
                enterButton.interactable = false;
            }
        }

        void OnEnterButtonClicked()
        {
            provider.JoinSessionAsync();
        }

        void OnStartedSession(PurrLobby.Lobby lobby)
        {
            OnStartedSession();
        }

        void OnStartedSession()
        {
            inputField.interactable = false;
            enterButton.interactable = false;
        }

        void OnLeavedSession(string l)
        {
            OnLeavedSession();
        }

        void OnLeavedSession()
        {
            inputField.interactable = true;
            OnUpdateField(inputField.text);
        }
    }
}