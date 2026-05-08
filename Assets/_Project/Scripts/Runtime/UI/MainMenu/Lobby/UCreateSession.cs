using ModestTree;
using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCreateSession : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private Button createButton;

        SessionProvider provider;
        string sessionName;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;
            nameInput.onValueChanged.AddListener(OnUpdateField);

            provider.OnAddingSessionStarted += OnStartedSession;
            provider.OnSessionAdded += OnStartedSession;
            provider.OnAddingSessionFailed += OnLeavedSession;
            provider.OnSessionLeaved += OnLeavedSession;

            createButton.onClick.AddListener(OnButtonClicked);

            var initizialSessionName = provider.GetSessionName();
            nameInput.text = initizialSessionName;

            SetInteractable(true);
        }

        void OnDestroy()
        {
            provider.OnAddingSessionStarted -= OnStartedSession;
            provider.OnSessionAdded -= OnStartedSession;
            provider.OnAddingSessionFailed -= OnLeavedSession;
            provider.OnSessionLeaved -= OnLeavedSession;
        }

        void OnUpdateField(string value)
        {
            sessionName = value;
            if (sessionName != null && sessionName.Length > 0)
            {
                provider.SetSessionName(sessionName);
                createButton.interactable = nameInput.interactable;
            }
            else
            {
                createButton.interactable = false;
            }
        }

        void OnButtonClicked()
        {
            provider.CreateSessionAsync();
        }

        void OnStartedSession(PurrLobby.Lobby lobby)
        {
            OnStartedSession();
        }

        void OnStartedSession()
        {
            SetInteractable(false);
        }

        void OnLeavedSession(string l)
        {
            OnLeavedSession();
        }

        void OnLeavedSession()
        {
            SetInteractable(true);
        }

        void SetInteractable(bool value)
        {
            nameInput.interactable = value;
            createButton.interactable = value;

            OnUpdateField(sessionName);
        }
    }
}