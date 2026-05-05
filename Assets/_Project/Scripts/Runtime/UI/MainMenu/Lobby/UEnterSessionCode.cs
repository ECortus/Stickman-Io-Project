using ModestTree;
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

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;

            provider.OnAddingSessionStarted += (e) => OnStartedSession();
            provider.OnSessionAdded += (e) => OnStartedSession();

            provider.OnAddingSessionFailed += (e, t) => OnLeavedSession();
            provider.OnSessionLeaved += OnLeavedSession;

            inputField.onValueChanged.AddListener(OnUpdateField);
            enterButton.onClick.AddListener(OnEnterButtonClicked);

            OnLeavedSession();
            OnUpdateField();
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

        async void OnEnterButtonClicked()
        {
            await provider.JoinSessionAsync();
        }

        void OnStartedSession()
        {
            inputField.interactable = false;
            enterButton.interactable = false;
        }

        void OnLeavedSession()
        {
            inputField.interactable = true;
            OnUpdateField(inputField.text);
        }
    }
}