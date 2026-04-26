using System.Collections.Generic;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.MainMenu;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace StickmanIo.Runtime.UI
{
    public class USkinButtonsManager : MonoBehaviour
    {
        [SerializeField] private USkinButton skinButtonPrefab;
        [SerializeField] private RectTransform buttonsParent;

        List<USkinButton> skinButtons = new List<USkinButton>();

        PlayerSkinProvider skinProvider;
        GoldStorage goldStorage;

        void Start()
        {
            skinProvider = PlayerSkinProvider.GetInstance;
            goldStorage = GoldStorage.GetInstance;

            SetupButtons();

            goldStorage.onChanged += OnUpdateGold;
        }

        void OnDestroy()
        {
            goldStorage.onChanged -= OnUpdateGold;
        }

        void SetupButtons()
        {
            buttonsParent.DestroyAllChildren();

            var skins = skinProvider.GetSkinsRuntimeData();
            for (int i = 0; i < skins.Count; i++)
            {
                var runtimeData = skins[i];

                var buttonInstance = ObjectInstantiator.InstantiatePrefabForComponent(skinButtonPrefab, buttonsParent);
                buttonInstance.SetupButton(runtimeData);

                skinButtons.Add(buttonInstance);
            }
        }

        public void UnequipAllExcept(USkinButton equippedRuntime)
        {
            foreach (var button in skinButtons)
            {
                if (button == equippedRuntime) continue;
                button.SetIsEquipped(false);
            }
        }

        void OnUpdateGold()
        {
            UpdateButtons();
        }

        void UpdateButtons()
        {
            foreach (var button in skinButtons)
            {
                button.UpdateButton();
            }
        }
    }
}