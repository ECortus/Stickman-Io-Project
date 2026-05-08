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

        private void Start()
        {
            layersController = FindAnyObjectByType<USettingsLayersController>();
            
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            layersController.PreviousLayer();
            if (layersController.Layer == USettingsLayersController.ELayer.Off)
            {
                mainMenu ??= FindAnyObjectByType<UMainMenu>(FindObjectsInactive.Include);
                mainMenu.OpenMainMenuPanel();
            }
        }
    }
}