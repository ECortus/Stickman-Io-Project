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

        [Space(10)]
        [SerializeField, ReadOnly] string SessionName = "";
        [SerializeField, ReadOnly] string SessionCode = "";

        SessionObserver m_SessionObserver;
        ISession m_Session;

        void Awake() 
        {
            Initialize();
        }

        void Initialize() 
        {
            var options = settings.ToSessionOptions();
            m_SessionObserver = new SessionObserver(options.Type);

            m_SessionObserver.AddingSessionStarted += OnAddingSessionStartedMethod;
            m_SessionObserver.AddingSessionFailed += OnAddingSessionFailedMethod;
            m_SessionObserver.SessionAdded += OnSessionAddedMethod;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_SessionObserver == null)
            {
                return;
            }

            m_SessionObserver.AddingSessionStarted -= OnAddingSessionStartedMethod;
            m_SessionObserver.AddingSessionFailed -= OnAddingSessionFailedMethod;
            m_SessionObserver.SessionAdded -= OnSessionAddedMethod;

            m_SessionObserver.Dispose();
        }

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

        public void SetSession(ISession session)
        {
            m_Session = session;
        }

        public ISession GetSession()
        {
            return m_Session;
        }

        bool AreMultiplayerServicesInitialized()
        {
            return MultiplayerService.Instance != null;
        }

        public async Task CreateSessionAsync()
        {
            if (!AreMultiplayerServicesInitialized())
            {
                throw new Exception("Multiplayer services are not initialized.");
            }

            var sessionOptions = settings.ToSessionOptions();
            await CreateSessionAsync(sessionOptions);
        }

        public async Task JoinSessionAsync()
        {
            if (!AreMultiplayerServicesInitialized())
            {
                throw new Exception("Multiplayer services are not initialized.");
            }

            var joinSessionOptions = settings.ToJoinSessionOptions();
            await JoinSessionByCodeAsync(joinSessionOptions);
        }

        public async Task LeaveSessionAsync()
        {
            if (m_Session == null)
            {
                DebugHelper.LogWarning("No session to leave.");
                return;
            }

            OnSessionLeavedMethod();
            await m_Session.LeaveAsync();
        }

        async Task<IHostSession> CreateSessionAsync(SessionOptions sessionOptions)
        {
            sessionOptions.Name = SessionName;
            sessionOptions.WithPurrRelay();

            return await MultiplayerService.Instance.CreateSessionAsync(sessionOptions);
        }

        async Task<ISession> JoinSessionByCodeAsync(JoinSessionOptions joinSessionOptions)
        {
            return await MultiplayerService.Instance.JoinSessionByCodeAsync(SessionCode, joinSessionOptions);
        }

        #region Events

        public event Action<AddingSessionOptions> OnAddingSessionStarted;
        public event Action<AddingSessionOptions, SessionException> OnAddingSessionFailed;
        public event Action<ISession> OnSessionAdded;

        void OnAddingSessionStartedMethod(AddingSessionOptions sessionOptions)
        {
            OnAddingSessionStarted?.Invoke(sessionOptions);
        }        

        void OnAddingSessionFailedMethod(AddingSessionOptions sessionOptions, SessionException exception)
        {
            OnAddingSessionFailed?.Invoke(sessionOptions, exception);
        }

        void OnSessionAddedMethod(ISession session)
        {
            SetSession(session);
            SetSessionCode(session.Code);

            OnSessionAdded?.Invoke(session);
        }
        
        public event Action OnSessionLeaved;

        void OnSessionLeavedMethod()
        {
            SetSession(null);
            OnSessionLeaved?.Invoke();
        }

        #endregion
    }
}