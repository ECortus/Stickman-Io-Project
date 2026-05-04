using System.Threading.Tasks;
using Blocks.Sessions.Common;
using GameDevUtils.Runtime;
using PurrNet.MultiplayerServices;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace StickmanIo.Runtime.MainMenu.Lobby
{
    public class SessionProvider : SingletonMonoBehaviour<SessionProvider>
    {
        [SerializeField] private SessionSettings settings;

        public bool SessionStarted { get; private set; }

        string SessionName; // TODO: Set Name
        string SessionCode; // TODO: Set Code

        public void SetSessionName(string sessionName)
        {
            SessionName = sessionName;
        }

        public string GetSessionName()
        {
            return SessionName;
        }

        public void SetSessionCode(string sessionCode)
        {
            SessionCode = sessionCode;
        }

        public string GetSessionCode()
        {
            return SessionCode;
        }

        public async Task<IHostSession> CreateSessionAsync(SessionOptions sessionOptions)
        {
            sessionOptions.Name = SessionName;
            sessionOptions = sessionOptions.WithPurrRelay();

            return await MultiplayerService.Instance.CreateSessionAsync(sessionOptions);
        }

        public async Task<ISession> JoinSessionByCodeAsync(JoinSessionOptions joinSessionOptions)
        {
            return await MultiplayerService.Instance.JoinSessionByCodeAsync(SessionCode, joinSessionOptions);
        }
    }
}