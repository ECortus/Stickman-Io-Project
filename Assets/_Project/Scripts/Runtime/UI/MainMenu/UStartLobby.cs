using StickmanIo.Runtime.MainMenu.Lobby;
using StickmanIo.Runtime.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UStartLobby : MonoBehaviour
    {
        [SerializeField] private Button startPlayButton;
        [SerializeField] private Button toMainMenuButton;

        IMainMenu mainMenu;

        SessionProvider sessionProvider;

        void Awake()
        {
            mainMenu = GetComponentInParent<IMainMenu>();
            sessionProvider = SessionProvider.GetInstance;

            sessionProvider.OnSessionStarted += OnStartedSession;
            sessionProvider.OnSessionLeaved += OnLeavedSession;

            SetupButtons();
        }

        void SetupButtons()
        {
            startPlayButton.onClick.AddListener(OnStartPlayButton);
            toMainMenuButton.onClick.AddListener(OnToMainMenuButton);

            startPlayButton.interactable = false;
        }

        void OnStartPlayButton()
        {
            ProjectSceneLoader.LoadGameplayScene();
        }

        void OnToMainMenuButton()
        {
            mainMenu.OpenMainMenuPanel();
        }

        void OnStartedSession()
        {
            startPlayButton.interactable = true;
        }

        void OnLeavedSession()
        {
            startPlayButton.interactable = false;
        }
    }
}