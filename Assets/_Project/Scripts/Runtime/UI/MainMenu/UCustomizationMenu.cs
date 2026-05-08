using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCustomizationMenu : MonoBehaviour
    {
        [SerializeField] private Button toMainMenuButton;

        IMainMenu mainMenu;

        void Start()
        {
            mainMenu = GetComponentInParent<IMainMenu>();
            SetupButtons();
        }

        void SetupButtons()
        {
            toMainMenuButton.onClick.AddListener(OnToMainMenuButton);
        }

        void OnToMainMenuButton()
        {
            mainMenu.OpenMainMenuPanel();
        }
    }
}