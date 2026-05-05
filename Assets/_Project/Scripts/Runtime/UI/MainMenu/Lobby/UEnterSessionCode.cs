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

            inputField.onValueChanged.AddListener(OnUpdateField);
            enterButton.onClick.AddListener(OnEnterButtonClicked);

            OnUpdateField();
        }

        void OnUpdateField(string value = "")
        {
            if (value.IsEmpty())
            {
                enterButton.interactable = false;
                return;
            }
            else
            {
                provider.SetSessionCode(value);
                enterButton.interactable = true;
            }
        }

        async void OnEnterButtonClicked()
        {
            await provider.JoinSessionAsync();
        }
        
    }
}