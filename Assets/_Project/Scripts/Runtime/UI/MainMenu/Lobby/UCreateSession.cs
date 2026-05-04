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
            provider = SessionProvider.GetInstance;
            nameInput.onValueChanged.AddListener(OnUpdateField);

            var initizialSessionName = provider.GetSessionName();
            nameInput.text = initizialSessionName;
            OnUpdateField(initizialSessionName);
        }

        void Update()
        {
            if (provider.SessionStarted)
            {
                nameInput.interactable = false;
                createButton.interactable = false;
                return;
            }
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

        void OnButtonClicked()
        {
            
        }
    }
}