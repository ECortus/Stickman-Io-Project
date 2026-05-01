using SaveableExtension.Runtime;
using SettingsMenu.Runtime;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public interface IMainMenu
    {
        void OpenMainMenuPanel();
    }

    public class UMainMenu : MonoBehaviour, IMainMenu
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject customizationPanel;

        [Space(5)]
        [SerializeField] private GameObject startLobbyPanel;

        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button customizationButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        SettingsMenuManager settingsMenuManager;

        void Awake()
        {
            SetupButtons();
        }

        void Start()
        {
            OpenMainMenuPanel();
        }

        void SetupButtons()
        {
            startButton.onClick.AddListener(OpenStartLobbyPanel);
            customizationButton.onClick.AddListener(OpenCustomizationPanel);
            settingsButton.onClick.AddListener(OpenSettingsPanel);
            quitButton.onClick.AddListener(QuitApplication);
        }

        public void OpenMainMenuPanel()
        {
            CloseAll();
            SetMainMenuPanelActive(true);

            SaveablePrefs.Save<ProjectSavePrefs>(true);
        }

        void OpenStartLobbyPanel()
        {
            CloseAll();
            SetStartLobbyPanelActive(true);
        }

        void OpenCustomizationPanel()
        {
            CloseAll();
            SetCustomizationPanelActive(true);
        }

        void OpenSettingsPanel()
        {
            CloseAll();
            SetSettingsPanelActive(true);
        }

        void CloseAll()
        {
            SetMainMenuPanelActive(false);
            SetCustomizationPanelActive(false);
            SetSettingsPanelActive(false);

            SetStartLobbyPanelActive(false);
        }

        void SetMainMenuPanelActive(bool active)
        {
            mainMenuPanel.SetActive(active);
        }

        void SetCustomizationPanelActive(bool active)
        {
            customizationPanel.SetActive(active);
        }

        void SetSettingsPanelActive(bool active)
        {
            settingsMenuManager ??= FindAnyObjectByType<SettingsMenuManager>();

            if (active)
            {
                settingsMenuManager.On();
            }
            else
            {
                settingsMenuManager.Off();
            }
        }

        void SetStartLobbyPanelActive(bool active)
        {
            startLobbyPanel.SetActive(active);
        }

        void QuitApplication()
        {
            Application.Quit();
        }
    }
}