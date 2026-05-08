using StickmanIo.Runtime.MainMenu.Lobby;
using StickmanIo.Runtime.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UStartLobby : MonoBehaviour
    {
        [SerializeField] private Button startPlayButton;
        [SerializeField] private Button toMainMenuButton;

        TMP_Text isReadyText;

        IMainMenu mainMenu;

        SessionProvider sessionProvider;

        void Start()
        {
            mainMenu = GetComponentInParent<IMainMenu>();
            sessionProvider = SessionProvider.GetInstance;

            isReadyText = startPlayButton.GetComponentInChildren<TMP_Text>();

            sessionProvider.OnSessionAdded += OnStartedSession;
            sessionProvider.OnSessionLeaved += OnLeavedSession;

            SetupButtons();
        }

        void OnDestroy()
        {
            sessionProvider.OnSessionAdded -= OnStartedSession;
            sessionProvider.OnSessionLeaved -= OnLeavedSession;
        }

        void SetupButtons()
        {
            startPlayButton.onClick.AddListener(OnStartPlayButton);
            toMainMenuButton.onClick.AddListener(OnToMainMenuButton);

            isReadyText.text = "Start Play";
            startPlayButton.interactable = false;
        }

        void OnStartPlayButton()
        {
            isReadyText.text = "Wait all ready...";
            startPlayButton.interactable = false;

            sessionProvider.SetIsReady();
        }

        void OnToMainMenuButton()
        {
            mainMenu.OpenMainMenuPanel();
        }

        void OnStartedSession(PurrLobby.Lobby lobby)
        {
            OnStartedSession();
        }

        void OnStartedSession()
        {
            startPlayButton.interactable = true;
        }

        void OnLeavedSession(string l)
        {
            OnLeavedSession();
        }

        void OnLeavedSession()
        {
            isReadyText.text = "Start Play";
            startPlayButton.interactable = false;
        }
    }
}