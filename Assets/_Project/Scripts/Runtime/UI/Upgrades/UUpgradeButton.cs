using System;
using StickmanIo.Runtime.Units;
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
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private Button upgradeButton;

        public event Action OnButtonClickEvent;

        public event Action OnDestroyEvent;

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
            var lvl = runtimeData.Level;
            if (lvl <= 0)
            {
                percentageText.gameObject.SetActive(false);
            }
            else
            {
                percentageText.gameObject.SetActive(true);

                var value = runtimeData.GetFullValue();
                percentageText.text = $"+{Mathf.RoundToInt(value * 100)}%";
            }

            levelText.text = $"{lvl:00}";
        }

        public void OnButtonClick()
        {
            runtimeData.Upgrade();
            UpdateButton();

            OnButtonClickEvent?.Invoke();
        }

        void OnDestroy() 
        {
            OnDestroyEvent?.Invoke();
        }
    }
}