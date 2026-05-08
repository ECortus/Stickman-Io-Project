using StickmanIo.Runtime.MainMenu.Lobby;
using UnityEngine;

namespace StickmanIo.Runtime.Core
{
    public class MainMenuSceneProvider : MonoBehaviour
    {
        SessionProvider sessionProvider;

        void Awake()
        {
            sessionProvider = SessionProvider.GetInstance;
            sessionProvider.LeaveSessionIfHasOne();
        }
    }
}