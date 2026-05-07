using System.Collections.Generic;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using PurrNet;
using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCurrentSession : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionLabel;
        [SerializeField] private string defaultSessionLabel = "No Session Joined";

        [Space(5)]
        [SerializeField] private TMP_Text sessionCurrentPlayerLabel;
        [SerializeField] private string defaultSessionOwnerLabel = "No Player Joined";

        [Space(5)]
        [SerializeField] private TMP_Text sessionPlayersCountLabel;
        [SerializeField] private string defaultSessionPlayersCountLabel = "-/- players";

        [Space(5)]
        [SerializeField] private TMP_Text sessionCodeLabel;
        [SerializeField] private Button copySessionCodeButton;
        [SerializeField] private string defaultSessionCodeLabel = "No Session Code";

        [Space(5)]
        [SerializeField] private TMP_Text playerListElementPrefab;
        [SerializeField] private RectTransform playersListParent;

        [Space(5)]
        [SerializeField] private Button leaveSessionButton;
        [SerializeField] private TMP_Text sessionStatusLabel;
        [SerializeField] private string defaultSessionStatusLabel = "In Session: No";

        SessionProvider provider;
        List<TMP_Text> playerList = new List<TMP_Text>();

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;

            provider.OnSessionAdded += (e) => OnStartedSession();
            provider.OnAddingSessionFailed += (e) => OnFailedSession(e);

            provider.OnSessionLeaved += OnLeavedSession;

            leaveSessionButton.onClick.AddListener(OnLeaveButtonClick);

            copySessionCodeButton.onClick.AddListener(OnCopySessionCodeButton);

            OnLeavedSession();
        }

        bool update = false;

        void Update()
        {
            if (!update)
            {
                return;
            }

            var manager = NetworkManager.main;
            if (manager == null)
            {
                return;
            }

            sessionStatusLabel.text = $"Server: {manager.serverState}, Client: {manager.clientState}";
        }

        void OnStartedSession()
        {
            var session = provider.GetSession();
            sessionLabel.text = session.Name;

            playersListParent.DestroyAllChildren();
            playerList.Clear();
            
            var lobbyManager = provider.LobbyManager;
            lobbyManager.OnPlayerListUpdated.AddListener(OnPlayersUpdated);

            foreach (var player in session.Members)
            {
                var playerID = player.Id;
                OnPlayerAdded(playerID);
            }

            sessionCodeLabel.text = $"{provider.GetSessionCode()}";

            copySessionCodeButton.interactable = true;
            leaveSessionButton.interactable = true;

            update = true;
        }

        void OnPlayersUpdated(List<PurrLobby.LobbyUser> users)
        {
            playersListParent.DestroyAllChildren();
            playerList.Clear();

            var session = provider.GetSession();
            foreach (var player in session.Members)
            {
                OnPlayerAdded(player.DisplayName);
            }
        }

        void OnPlayerAdded(string playerName)
        {
            var instance = ObjectInstantiator.InstantiatePrefabForComponent(playerListElementPrefab, playersListParent);
            instance.text = playerName;

            var local = instance.transform.localPosition;
            local.z = 0f;
            instance.transform.localPosition = local;

            playerList.Add(instance);

            UpdateDynamicFields();
        }

        void UpdateDynamicFields()
        {
            var session = provider.GetSession();
            if (session.Equals(default))
            {
                sessionCurrentPlayerLabel.text = defaultSessionOwnerLabel;
                sessionPlayersCountLabel.text = defaultSessionPlayersCountLabel;

                return;
            }

            sessionCurrentPlayerLabel.text = session.LobbyId;
            sessionPlayersCountLabel.text = $"{session.Members.Count}/{session.MaxPlayers} players";
        }

        void OnFailedSession(string exception)
        {
            OnLeavedSession();
            sessionStatusLabel.text = exception;
        }

        void OnLeavedSession()
        {
            sessionLabel.text = defaultSessionLabel;
            sessionCurrentPlayerLabel.text = defaultSessionOwnerLabel;
            sessionPlayersCountLabel.text = defaultSessionPlayersCountLabel;
            sessionCodeLabel.text = defaultSessionCodeLabel;
            sessionStatusLabel.text = defaultSessionStatusLabel;

            playersListParent.DestroyAllChildren();

            copySessionCodeButton.interactable = false;
            leaveSessionButton.interactable = false;

            update = false;
        }

        void OnCopySessionCodeButton()
        {
            if (string.IsNullOrEmpty(sessionCodeLabel.text) || sessionCodeLabel.text == defaultSessionCodeLabel)
            {
                return;
            }

            GUIUtility.systemCopyBuffer = sessionCodeLabel.text;
        }

        async void OnLeaveButtonClick()
        {
            leaveSessionButton.interactable = false;
            await provider.LeaveSessionAsync();
        }
    }
}