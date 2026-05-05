using System.Collections.Generic;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
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

        void OnStartedSession()
        {
            var session = provider.GetSession();

            sessionLabel.text = session.Name;
            sessionCurrentPlayerLabel.text = session.CurrentPlayer.GetPlayerName();
            sessionPlayersCountLabel.text = $"{session.PlayerCount}/{session.MaxPlayers} players";

            sessionStatusLabel.text = "In Session: Connected";

            playersListParent.DestroyAllChildren();

            session.PlayerJoined += OnPlayerAdded;
            session.PlayerLeaving += OnPlayerRemoved;

            foreach (var player in session.Players)
            {
                OnPlayerAdded(player.GetPlayerName());
            }

            sessionCodeLabel.text = $"{provider.GetSessionCode()}";

            copySessionCodeButton.interactable = true;
            leaveSessionButton.interactable = true;
        }

        void OnPlayerAdded(string player)
        {
            var instance = ObjectInstantiator.InstantiatePrefabForComponent(playerListElementPrefab, playersListParent);
            instance.text = player;

            playerList.Add(instance);
        }

        void OnPlayerRemoved(string player)
        {
            if (playerList.Count == 0)
            {
                return;
            }

            for (var i = 0; i < playerList.Count; i++)
            {
                if (playerList[i] && playerList[i].text == player)
                {
                    playerList.RemoveAt(i);
                    ObjectHelper.Destroy(playerList[i].gameObject);

                    break;
                }
                else if (!playerList[i])
                {
                    playerList.RemoveAt(i);
                    break;
                }
            }
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