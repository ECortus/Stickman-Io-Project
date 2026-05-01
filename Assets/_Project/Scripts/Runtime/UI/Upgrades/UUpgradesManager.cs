using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.Units;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

        [Space(10)]
        [SerializeField] private InputActionReference inputKey1;
        [SerializeField] private InputActionReference inputKey2;
        [SerializeField] private InputActionReference inputKey3;
        [SerializeField] private InputActionReference inputKey4;
        [SerializeField] private InputActionReference inputKey5;
        [SerializeField] private InputActionReference inputKey6;
        [SerializeField] private InputActionReference inputKey7;
        [SerializeField] private InputActionReference inputKey8;

        List<UUpgradeButton> buttons = new List<UUpgradeButton>();

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

                button.SetupButton(i, d);
                button.OnButtonClickEvent += ReduceAvailableUpgrades;

                BindButtonToInput(i, button);
            }
        }

        UUpgradeButton InstantiateButton()
        {
            var button = ObjectInstantiator.InstantiatePrefabForComponent(buttonPrefab, parentTransform);
            return button;
        }

        void BindButtonToInput(int id, UUpgradeButton button)
        {
            if (id < 0)
            {
                DebugHelper.LogError("Wrong id: " + id);
                return;
            }

            switch (id)
            {
                case 0:
                    inputKey1.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey1.action.performed -= OnKeyPerformed;
                    break;
                case 1:
                    inputKey2.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey2.action.performed -= OnKeyPerformed;
                    break;
                case 2:
                    inputKey3.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey3.action.performed -= OnKeyPerformed;
                    break;
                case 3:
                    inputKey4.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey4.action.performed -= OnKeyPerformed;
                    break;
                case 4:
                    inputKey5.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey5.action.performed -= OnKeyPerformed;
                    break;
                case 5:
                    inputKey6.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey6.action.performed -= OnKeyPerformed;
                    break;
                case 6:
                    inputKey7.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey7.action.performed -= OnKeyPerformed;
                    break;
                case 7:
                    inputKey8.action.performed += OnKeyPerformed;
                    button.OnDestroyEvent += () => inputKey8.action.performed -= OnKeyPerformed;
                    break;
                default:
                    break;
            }

            void OnKeyPerformed(InputAction.CallbackContext context)
            {
                var upgrades = ownerRig.Upgrades;
                if (upgrades.HasAvailableUpgrade())
                {
                    button.OnButtonClick();
                }
            }
        }
    }
}