using MultiSceneController.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.SceneManagement
{
    public static class ProjectSceneLoader
    {
        const string MainMenu_Section = "MainMenu";
        const string GamePlay_Section = "Gameplay";

        const string MainMenu_Scene = "MainMenu";
        const string GamePlay_Scene = "GameplayScene";

        public static void LoadMainMenu()
        {
            SceneLoader.NewTransition()
                .Load(MainMenu_Section, MainMenu_Scene, setActive: true)
                .Unload(GamePlay_Section)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        public static void LoadGameplayScene()
        {
            SceneLoader.NewTransition()
                .Load(GamePlay_Section, GamePlay_Scene, setActive: true)
                .Unload(MainMenu_Section)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        public static void QuitApplication()
        {
            Application.Quit();
        }
    }
}