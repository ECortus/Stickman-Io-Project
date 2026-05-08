using StickmanIo.Runtime.MainMenu.Lobby;
using UnityEngine;

namespace StickmanIo.Runtime.Core
{
    public class MainMenuSceneProvider : MonoBehaviour
    {
        SessionProvider sessionProvider;

        async void Awake()
        {
            sessionProvider = SessionProvider.GetInstance;
            await sessionProvider.LeaveSessionIfHasOne();
        }
    }
}