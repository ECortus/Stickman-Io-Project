using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCopySessionCode : MonoBehaviour
    {
        const float copiedTitleDuration = 2f;

        [SerializeField] TMP_InputField inputField;
        [SerializeField] Button copyButton;

        TMP_Text copyButtonText;

        SessionProvider provider;

        float copiedTitleTime;

        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            if (!copyButtonText)
            {
                return;
            }

            if (copiedTitleTime > 0)
            {
                copiedTitleTime -= Time.deltaTime;
                copyButtonText.text = "Copied!";
            }
            else
            {
                copyButtonText.text = "Copy";
            }
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;

            copyButtonText = copyButton.GetComponentInChildren<TMP_Text>();

            provider.OnSessionAdded += (e) => OnStartedSession();

            provider.OnAddingSessionFailed += (e, t) => OnLeavedSession();
            provider.OnSessionLeaved += OnLeavedSession;

            inputField.interactable = false;
            copyButton.onClick.AddListener(OnCopyButton);

            OnLeavedSession();
        }

        void OnStartedSession()
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
            copiedTitleTime = copiedTitleDuration;
            GUIUtility.systemCopyBuffer = inputField.text;
        }
    }
}