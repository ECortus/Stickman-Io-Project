using PurrNet;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.SceneManagement;
using StickmanIo.Runtime.Units;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UDeathScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text killsText;

        [SerializeField] private Button respawnButton;
        [SerializeField] private Button mainMenuButton;

        GameStatement gameStatement;
        UnitsManager unitsManager;

        PlayerID? playerID;

        void Start()
        {
            gameStatement = GameStatement.GetInstance;

            unitsManager = UnitsManager.GetInstance;
            unitsManager.OnOwnerRigChanged += OnOwnerRigChanged;
            OnOwnerRigChanged();

            SetActive(false);
        }

        void SetActive(bool active)
        {
            root.SetActive(active);
        }

        void OnOwnerRigChanged()
        {
            var player = unitsManager.OwnerRig;
            if (player != null)
            {
                player.Health.OnDiedRig += OnDied;
            }
        }

        void OnDied(PlayerRig player)
        {
            SetActive(true);
            gameStatement.SetDead();

            playerID = player.localPlayer;

            SetupTexts(player);
            SetupButtons();
        }

        void SetupTexts(PlayerRig rig)
        {
            var score = rig.Resources.Score;
            var kills = rig.Level.Level;
            
            scoreText.text = $"Score: {score}";
            killsText.text = $"Kills: {kills}";
        }

        void SetupButtons()
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        void OnRespawnButtonClicked()
        {
            var respawner = PlayerRespawner.GetInstance;
            respawner.RespawnPlayer(playerID);
            
            if (gameStatement.IsDead)
            {
                gameStatement.SetPlay();
            }
            
            SetActive(false);
        }

        void OnMainMenuButtonClicked()
        {
            ProjectSceneLoader.LoadMainMenu();
        }
    }
}