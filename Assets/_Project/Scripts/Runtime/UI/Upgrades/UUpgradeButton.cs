using System;
using StickmanIo.Runtime.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UUpgradeButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button upgradeButton;

        public event Action OnButtonClickEvent;

        UpgradeData data;
        UpgradeRuntimeData runtimeData;

        public void SetupButton(UpgradeRuntimeData rd)
        {
            runtimeData = rd;
            data = rd.Data;

            labelText.text = data.title;
            iconImage.sprite = data.icon;

            UpdateButton();

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnButtonClick);
        }

        void UpdateButton()
        {
            levelText.text = $"{runtimeData.Level:00}";
        }

        void OnButtonClick()
        {
            runtimeData.AddLevel();
            UpdateButton();

            OnButtonClickEvent?.Invoke();
        }
    }
}