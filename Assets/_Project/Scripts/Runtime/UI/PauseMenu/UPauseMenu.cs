using StickmanIo.Runtime.Input;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;

        [Space(5)]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        [Space(10)]
        [SerializeField] private InputActionReference pauseAction;

        GameStatement gameStatement;

        bool isOpened = false;

        void Awake()
        {
            gameStatement = GameStatement.GetInstance;

            SetupButtons();
            SetupInputs();
        }

        void SetupButtons()
        {
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        void SetupInputs()
        {
            pauseAction.action.performed += OnPauseActionPerformed;
        }

        void OnDestroy()
        {
            pauseAction.action.performed -= OnPauseActionPerformed;
        }

        void OnPauseActionPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isOpened = !isOpened;
                if (isOpened)
                {
                    OnPauseButtonClicked();
                }
                else
                {
                    OnResumeButtonClicked();
                }
            }
        }

        void OnPauseButtonClicked()
        {
            isOpened = true;
            SetMenuRootActive(isOpened);

            gameStatement.SetPause();
        }

        void OnResumeButtonClicked()
        {
            isOpened = false;
            SetMenuRootActive(false);

            gameStatement.SetPlay();
        }

        void SetMenuRootActive(bool active)
        {
            menuRoot.gameObject.SetActive(active);
        }

        void OnMainMenuButtonClicked()
        {
            ProjectSceneLoader.LoadMainMenu();
        }

        void OnQuitButtonClicked()
        {
            ProjectSceneLoader.QuitApplication();
        }
    }
}