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
            provider.OnAddingSessionFailed += (e, t) => OnFailedSession(e, t);

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

            session.PlayerJoined += OnPlayerAddedByID;
            session.PlayerLeaving += OnPlayerRemovedByID;

            foreach (var player in session.Players)
            {
                var playerID = player.Id;
                OnPlayerAddedByID(playerID);
            }

            sessionCodeLabel.text = $"{provider.GetSessionCode()}";

            copySessionCodeButton.interactable = true;
            leaveSessionButton.interactable = true;

            update = true;
        }

        void OnPlayerAddedByID(string playerID)
        {
            var session = provider.GetSession();
            if (session == null)
            {
                return;
            }

            var player = session.GetPlayer(playerID);
            var playerName = player.GetPlayerName();

            OnPlayerAdded(playerName);
        }

        void OnPlayerAddedByName(string playerName)
        {
            OnPlayerAdded(playerName);
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

        void OnPlayerRemovedByID(string playerID)
        {
            var session = provider.GetSession();
            if (session == null)
            {
                return;
            }

            var player = session.GetPlayer(playerID);
            if (player == null)
            {
                return;
            }

            var playerName = player.GetPlayerName();
            OnPlayerRemoved(playerName);
        } 

        void OnPlayerRemovedByName(string playerName)
        {
            OnPlayerRemoved(playerName);
        }

        void OnPlayerRemoved(string playerName)
        {
            if (playerList.Count == 0)
            {
                return;
            }

            for (var i = 0; i < playerList.Count; i++)
            {
                var item = playerList[i];
                if (item && item.text == playerName)
                {
                    playerList.RemoveAt(i);
                    ObjectHelper.Destroy(item.gameObject);

                    break;
                }
                else if (!item)
                {
                    playerList.RemoveAt(i);
                    break;
                }
            }

            UpdateDynamicFields();
        }

        void UpdateDynamicFields()
        {
            var session = provider.GetSession();
            if (session == null)
            {
                sessionCurrentPlayerLabel.text = defaultSessionOwnerLabel;
                sessionPlayersCountLabel.text = defaultSessionPlayersCountLabel;
                return;
            }

            sessionCurrentPlayerLabel.text = session.CurrentPlayer == null ? defaultSessionOwnerLabel : session.CurrentPlayer.GetPlayerName();
            sessionPlayersCountLabel.text = $"{playerList.Count}/{session.MaxPlayers} players";
        }

        void OnFailedSession(AddingSessionOptions sessionOptions, SessionException exception)
        {
            OnLeavedSession();
            sessionStatusLabel.text = exception.Message;
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