using SaveableExtension.Runtime;
using StickmanProject.Runtime.SavePrefs;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

namespace StickmanIo.Runtime.UI.MainMenu
{
    public class UStatisticsMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text maximumScoreText;
        [SerializeField] private TMP_Text maximumKillsText;

        ProjectSavePrefs prefs;

        void Start()
        {
            prefs = SaveablePrefs.LoadPrefs<ProjectSavePrefs>();
            UpdateTexts();
        }

        void UpdateTexts()
        {
            var score = prefs.MaximumScore;
            var kills = prefs.MaximumKills;
            
            maximumScoreText.text = $"Max Score: {score}";
            maximumKillsText.text = $"Max Kills: {kills}";
        }
    }
}