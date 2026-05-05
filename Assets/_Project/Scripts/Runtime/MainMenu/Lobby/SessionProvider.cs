using System;
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

        bool AreMultiplayerServicesInitialized()
        {
            return MultiplayerService.Instance != null;
        }

        public async Task CreateSessionAsync()
        {
            if (AreMultiplayerServicesInitialized())
            {
                OnSessionCreated?.Invoke();
            }
            else
            {
                OnSessionCreatedFailed?.Invoke();
                return;
            }

            var sessionOptions = settings.ToSessionOptions();
            var session = await CreateSessionAsync(sessionOptions);

            OnSessionStarted?.Invoke();
        }

        public async Task JoinSessionAsync()
        {
            if (AreMultiplayerServicesInitialized())
            {
                OnSessionJoined?.Invoke();
            }
            else
            {
                OnSessionJoinedFailed?.Invoke();
                return;
            }

            var joinSessionOptions = settings.ToJoinSessionOptions();
            var session = await JoinSessionByCodeAsync(joinSessionOptions);

            OnSessionStarted?.Invoke();
        }

        async Task<IHostSession> CreateSessionAsync(SessionOptions sessionOptions)
        {
            sessionOptions.Name = SessionName;
            sessionOptions = sessionOptions.WithPurrRelay();

            return await MultiplayerService.Instance.CreateSessionAsync(sessionOptions);
        }

        async Task<ISession> JoinSessionByCodeAsync(JoinSessionOptions joinSessionOptions)
        {
            return await MultiplayerService.Instance.JoinSessionByCodeAsync(SessionCode, joinSessionOptions);
        }

        #region Events
        
        public event Action OnSessionCreated;
        public event Action OnSessionCreatedFailed;

        public event Action OnSessionJoined;
        public event Action OnSessionJoinedFailed;

        public event Action OnSessionStarted;
        public event Action OnSessionLeaved;

        #endregion
    }
}