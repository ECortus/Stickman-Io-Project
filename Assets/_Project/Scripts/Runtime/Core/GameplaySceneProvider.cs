using PurrNet;
using PurrNet.Transports;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.SceneManagement;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Core
{
    public class GameplaySceneProvider : MonoBehaviour
    {
        void Awake() 
        {
            var statement = GameStatement.GetInstance;
            statement.SetLoading();

            NetworkManager.onAnyServerConnectionState += OnServerDisconnected;
            NetworkManager.onAnyClientConnectionState += OnClientDisconnected;
        }

        void OnDestroy()
        {
            NetworkManager.onAnyServerConnectionState -= OnServerDisconnected;
            NetworkManager.onAnyClientConnectionState -= OnClientDisconnected;
        }

        void OnServerDisconnected(ConnectionState state)
        {
            if (state == ConnectionState.Disconnecting)
            {
                Debug.Log("Disconnected from server");

                SaveablePrefs.Save<ProjectSavePrefs>(true);
                ProjectSceneLoader.LoadMainMenu();
            }
        }

        void OnClientDisconnected(ConnectionState state)
        {
            if (state == ConnectionState.Disconnecting)
            {
                Debug.Log("Disconnected from server");

                SaveablePrefs.Save<ProjectSavePrefs>(true);
                ProjectSceneLoader.LoadMainMenu();
            }
        }
    }
}