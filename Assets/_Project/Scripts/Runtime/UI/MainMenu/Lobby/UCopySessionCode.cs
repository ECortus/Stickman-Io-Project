using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCopySessionCode : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Button copyButton;

        SessionProvider provider;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;

            provider.OnSessionCreated += OnCreatedSession;
            provider.OnSessionLeaved += OnLeavedSession;

            copyButton.onClick.AddListener(OnCopyButton);

            inputField.text = "";
            copyButton.interactable = false;
        }

        void OnCreatedSession()
        {
            inputField.text = provider.GetSessionCode();
            copyButton.interactable = true;
        }

        void OnLeavedSession()
        {
            inputField.text = "";
            copyButton.interactable = false;
        }

        void OnCopyButton()
        {
            GUIUtility.systemCopyBuffer = inputField.text;
        }
    }
}