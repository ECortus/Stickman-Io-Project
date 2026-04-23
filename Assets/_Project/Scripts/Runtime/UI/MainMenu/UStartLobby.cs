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

        void Awake()
        {
            mainMenu = GetComponentInParent<IMainMenu>();
            SetupButtons();
        }

        void SetupButtons()
        {
            startPlayButton.onClick.AddListener(OnStartPlayButton);
            toMainMenuButton.onClick.AddListener(OnToMainMenuButton);
        }

        void OnStartPlayButton()
        {
            ProjectSceneLoader.LoadGameplayScene();
        }

        void OnToMainMenuButton()
        {
            mainMenu.OpenMainMenuPanel();
        }
    }
}