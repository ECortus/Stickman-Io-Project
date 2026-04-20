using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.Units;
using TMPro;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class UUpgradesManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text availableUpgradesText;
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(5)]
        [SerializeField] private Transform scaleRoot;
        [SerializeField] private float scaleOnAvailable = 1f;
        [SerializeField] private float scaleOnNonAvailable = 0.8f;

        [Space(5)]
        [SerializeField] private UUpgradeButton buttonPrefab;
        [SerializeField] private Transform parentTransform;

        UnitsManager unitsManager;
        PlayerRig ownerRig;

        bool initialized = false;

        int previousAvailableUpgrades = -1;

        void Start()
        {
            unitsManager = UnitsManager.GetInstance;
            AsyncTaskHelper.CreateTask(ScheduleStart);
        }

        async UniTask ScheduleStart()
        {
            while (!unitsManager.OwnerRig)
            {
                await UniTask.Yield();
            }

            ownerRig = unitsManager.OwnerRig;

            var upgrades = ownerRig.Upgrades;

            while (!upgrades.UpgradesInitialized)
            {
                await UniTask.Yield();
            }

            var runtimeUpgrades = upgrades.RuntimeUpgrades;
            ReinstantiateAllButtons(runtimeUpgrades);

            initialized = true;
        }

        void Update()
        {
            if (!initialized)
            {
                return;
            }

            var upgrades = ownerRig.Upgrades;
            var availableUpgrades = upgrades.AvailableUpgrades;

            if (availableUpgrades != previousAvailableUpgrades)
            {
                previousAvailableUpgrades = availableUpgrades;
                UpdateAvailableUpgrades(availableUpgrades);
            }
        }

        void UpdateAvailableUpgrades(int availableUpgrades)
        {
            SetAvailableUpgrades(availableUpgrades);
            if (availableUpgrades > 0)
            {
                scaleRoot.localScale = Vector3.one * scaleOnAvailable;
                canvasGroup.interactable = true;
            }
            else
            {
                scaleRoot.localScale = Vector3.one * scaleOnNonAvailable;
                canvasGroup.interactable = false;
            }
        }

        void ReduceAvailableUpgrades()
        {
            var upgrades = ownerRig.Upgrades;
            upgrades.ReduceAvailableUpgrades();

            var availableUpgrades = upgrades.AvailableUpgrades;

            UpdateAvailableUpgrades(availableUpgrades);
        }

        void SetAvailableUpgrades(int availableUpgrades)
        {
            availableUpgradesText.text = $"Available upgrades: {availableUpgrades:00}";
        }

        public void ReinstantiateAllButtons(List<UpgradeRuntimeData> data)
        {
            parentTransform.DestroyAllChildren();

            for (int i = 0; i < data.Count; i++)
            {
                var d = data[i];

                var button = InstantiateButton();
                button.SetupButton(d);
                button.OnButtonClickEvent += ReduceAvailableUpgrades;
            }
        }

        UUpgradeButton InstantiateButton()
        {
            var button = ObjectInstantiator.InstantiatePrefabForComponent(buttonPrefab, parentTransform);
            return button;
        }
    }
}