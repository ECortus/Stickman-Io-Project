using System;
using GameDevUtils.Runtime;
using PurrNet;
using StickmanIo.Runtime.Player;

namespace StickmanIo.Runtime
{
    public class PlayersLogger : NetworkIdentity
    {
        static PlayersLogger instance;

        public static PlayersLogger GetInstance => instance;
        public static bool HasInstance => instance;

        public event Action<string> OnLogInstantiated;

        void Awake()
        {
            instance = this;
        }

        [ObserversRpc(runLocally: true)]
        public static void LogAdded(string message)
        {
            if (!instance) return;

            message = $"{message} added";
            instance.OnLogInstantiated?.Invoke(message);
        }

        [ObserversRpc(runLocally: true)]
        public static void LogSpawned(string message)
        {
            if (!instance) return;

            message = $"{message} spawned";
            instance.OnLogInstantiated?.Invoke(message);
        }

        [ObserversRpc(runLocally: true)]
        public static void LogKilled(string message)
        {
            if (!instance) return;

            message = $"{message} is dead";
            instance.OnLogInstantiated?.Invoke(message);
        }
    }
}