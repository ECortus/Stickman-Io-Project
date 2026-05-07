using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;

namespace StickmanIo.Runtime
{
    public class PlayersLogger : SingletonMonoBehaviour<PlayersLogger>
    {
        public event Action<string> OnLogInstantiated;

        public static void LogAdded(string message)
        {
            message = $"{message} added";
            GetInstance.OnLogInstantiated?.Invoke(message);
        }

        public static void LogSpawned(string message)
        {
            message = $"{message} spawned";
            GetInstance.OnLogInstantiated?.Invoke(message);
        }

        public static void LogKilled(string message)
        {
            message = $"{message} is dead";
            GetInstance.OnLogInstantiated?.Invoke(message);
        }
    }
}