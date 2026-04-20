using System;
using StickmanIo.Runtime.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class UUpgradeButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text idText;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button upgradeButton;

        public event Action OnButtonClickEvent;

        int id;
        UpgradeData data;
        UpgradeRuntimeData runtimeData;

        public void SetupButton(int id, UpgradeRuntimeData rd)
        {
            runtimeData = rd;
            data = rd.Data;
            this.id = id;

            idText.text = (id + 1).ToString();
            labelText.text = data.title;
            iconImage.sprite = data.icon;

            UpdateButton();

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnButtonClick);
        }

        public void ManualUpgrade()
        {
            OnButtonClick();
        }

        void UpdateButton()
        {
            levelText.text = $"{runtimeData.Level:00}";
        }

        public void OnButtonClick()
        {
            runtimeData.AddLevel();
            UpdateButton();

            OnButtonClickEvent?.Invoke();
        }
    }
}