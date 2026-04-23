using UnityEngine;
using UnityEngine.UI;
using SettingsMenu.Runtime.UI.PC;

namespace StickmanIo.Runtime.UI
{
    public class USettingBackButton : MonoBehaviour
    {
        IMainMenu mainMenu;
        USettingsLayersController layersController;
        
        Button button;

        private void Awake()
        {
            layersController = FindAnyObjectByType<USettingsLayersController>();
            mainMenu = FindAnyObjectByType<UMainMenu>();
            
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            layersController.PreviousLayer();
            if (layersController.Layer == USettingsLayersController.ELayer.Off)
            {
                mainMenu.OpenMainMenuPanel();
            }
        }
    }
}