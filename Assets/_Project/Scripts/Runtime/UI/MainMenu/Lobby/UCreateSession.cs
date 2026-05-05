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

            provider.OnSessionCreated += () => SetInteractable(false);

            createButton.onClick.AddListener(OnButtonClicked);

            var initizialSessionName = provider.GetSessionName();
            nameInput.text = initizialSessionName;

            OnUpdateField(initizialSessionName);
        }

        void OnUpdateField(string value)
        {
            sessionName = value;
            if (!sessionName.IsEmpty())
            {
                provider.SetSessionName(sessionName);
                createButton.interactable = true;
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

        void SetInteractable(bool value)
        {
            nameInput.interactable = false;
            createButton.interactable = false;
        }
    }
}