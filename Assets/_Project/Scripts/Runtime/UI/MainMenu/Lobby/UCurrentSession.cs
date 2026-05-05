using StickmanIo.Runtime.MainMenu.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCurrentSession : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionLabel;
        [SerializeField] private string defaultSessionLabel = "No Session Joined";

        [Space(5)]
        [SerializeField] private TMP_Text sessionOwnerLabel;
        [SerializeField] private string defaultSessionOwnerLabel = "No Player Joined";

        [Space(5)]
        [SerializeField] private TMP_Text sessionPlayersCountLabel;
        [SerializeField] private string defaultSessionPlayersCountLabel = "-/- players";

        [Space(5)]
        [SerializeField] private TMP_Text sessionCodeLabel;
        [SerializeField] private Button copySessionCodeButton;
        [SerializeField] private string defaultSessionCodeLabel = "No Session Code";

        [Space(5)]
        [SerializeField] private RectTransform playerListElementPrefab;
        [SerializeField] private RectTransform playersListParent;

        [Space(5)]
        [SerializeField] private Button leaveSessionButton;
        [SerializeField] private TMP_Text sessionStatusLabel;
        [SerializeField] private string defaultSessionStatusLabel = "In Session: No";

        SessionProvider provider;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            provider = SessionProvider.GetInstance;
            copySessionCodeButton.onClick.AddListener(OnCopySessionCodeButton);
        }

        void OnCopySessionCodeButton()
        {
            if (string.IsNullOrEmpty(sessionCodeLabel.text) || sessionCodeLabel.text == defaultSessionCodeLabel)
            {
                return;
            }

            GUIUtility.systemCopyBuffer = sessionCodeLabel.text;
        }
    }
}