using System;
using System.Threading.Tasks;
using Blocks.Sessions.Common;
using GameDevUtils.Runtime;
using PurrLobby;
using PurrLobby.Providers;
using PurrNet;
using PurrNet.MultiplayerServices;
using PurrNet.Purrnity;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace StickmanIo.Runtime.MainMenu.Lobby
{
    public class SessionProvider : SingletonMonoBehaviour<SessionProvider>
    {
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private UnityLobbyProvider lobbyProvider;

        [Space(10)]
        [SerializeField, ReadOnly] string SessionName = "";
        [SerializeField, ReadOnly] string SessionID = "";

        ServicesInitializationProvider servicesInitializationProvider;

        PurrLobby.Lobby m_Session;

        public LobbyManager LobbyManager => lobbyManager;

        protected override void OnAwake() 
        {
            base.OnAwake();

            servicesInitializationProvider = ServicesInitializationProvider.GetInstance;
            servicesInitializationProvider.OnInitialized.AddListener(Initialize);
        }

        void Initialize() 
        {
            lobbyManager.OnRoomJoinFailed.AddListener(OnAddingSessionFailedMethod);
            lobbyManager.OnRoomJoined.AddListener(OnSessionAddedMethod);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            lobbyManager.OnRoomJoinFailed.RemoveListener(OnAddingSessionFailedMethod);
            lobbyManager.OnRoomJoined.RemoveListener(OnSessionAddedMethod);
        }

        public void SetSessionName(string sessionName)
        {
            SessionName = sessionName;
            lobbyProvider.lobbyName = sessionName;
        }

        public string GetSessionName()
        {
            return SessionName;
        }

        public void SetSessionCode(string sessionID)
        {
            SessionID = sessionID;
        }

        public string GetSessionCode()
        {
            return SessionID;
        }

        public void SetSession(PurrLobby.Lobby session)
        {
            m_Session = session;
        }

        public PurrLobby.Lobby GetSession()
        {
            return m_Session;
        }

        public async Task CreateSessionAsync()
        {
            OnAddingSessionStartedMethod();
            lobbyManager.CreateRoom(4, new System.Collections.Generic.Dictionary<string, string>() { { "LobbyName", SessionName } });
        }

        public async Task JoinSessionAsync()
        {
            lobbyManager.JoinLobby(SessionID);
        }

        public async Task LeaveSessionAsync()
        {
            if (m_Session.Equals(default))
            {
                DebugHelper.LogWarning("No session to leave.");
                return;
            }

            OnSessionLeavedMethod();
            lobbyManager.LeaveLobby(m_Session.LobbyId);

            SetSession(default);
        }

        #region Events

        public event Action OnAddingSessionStarted;
        public event Action<string> OnAddingSessionFailed;
        public event Action<PurrLobby.Lobby> OnSessionAdded;

        void OnAddingSessionStartedMethod()
        {
            OnAddingSessionStarted?.Invoke();
        }        

        void OnAddingSessionFailedMethod(string exception)
        {
            OnAddingSessionFailed?.Invoke(exception);
        }

        void OnSessionAddedMethod(PurrLobby.Lobby session)
        {
            SetSession(session);
            SetSessionCode(session.LobbyId);

            OnSessionAdded?.Invoke(session);
        }
        
        public event Action OnSessionLeaved;

        void OnSessionLeavedMethod()
        {
            OnSessionLeaved?.Invoke();
        }

        #endregion
    }
}