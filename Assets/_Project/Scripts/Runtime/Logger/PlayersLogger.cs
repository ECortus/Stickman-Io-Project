using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;

namespace StickmanIo.Runtime
{
    public class PlayersLogger : SingletonMonoBehaviour<PlayersLogger>
    {
        public event Action<string> OnLogInstantiated;

        public static void LogKilled(string killed)
        {
            var message = $"{killed} is dead";
            GetInstance.OnLogInstantiated?.Invoke(message);
        }
    }
}