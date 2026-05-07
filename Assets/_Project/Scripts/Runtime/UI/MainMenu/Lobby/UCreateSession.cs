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

        void Awake() 
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;
            nameInput.onValueChanged.AddListener(OnUpdateField);

            provider.OnAddingSessionStarted += OnStartedSession;
            provider.OnSessionAdded += (e) => OnStartedSession();

            provider.OnAddingSessionFailed += (e) => OnLeavedSession();
            provider.OnSessionLeaved += OnLeavedSession;

            createButton.onClick.AddListener(OnButtonClicked);

            var initizialSessionName = provider.GetSessionName();
            nameInput.text = initizialSessionName;

            SetInteractable(true);
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

        async void OnButtonClicked()
        {
            await provider.CreateSessionAsync();
        }

        void OnStartedSession()
        {
            SetInteractable(false);
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