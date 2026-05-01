using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.MainMenu;
using StickmanIo.Runtime.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class USkinButton : MonoBehaviour
    {
        [SerializeField] private RectTransform previewTransform;
        [SerializeField] private Material previewMaterial;

        [Space(5)]
        [SerializeField] private GameObject lockedObject;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text priceText;

        [Space(5)]
        [SerializeField] private GameObject ownedObject;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button equippedButton;

        bool equipped = false;
        bool isLocked = true;

        RectTransform rect;

        SkinData skinData;
        SkinDataRuntime dataRuntime;

        GoldStorage goldStorage;
        PlayerSkinProvider skinProvider;

        USkinButtonsManager manager;

        public void SetupButton(SkinDataRuntime runtime)
        {
            rect = GetComponent<RectTransform>();
            manager = GetComponentInParent<USkinButtonsManager>();

            dataRuntime = runtime;
            skinData = runtime.SkinData;

            goldStorage = GoldStorage.GetInstance;
            skinProvider = PlayerSkinProvider.GetInstance;

            SetIsLocked(!runtime.IsOwned);
            SetIsEquipped(runtime.IsEquipped);

            buyButton.onClick.AddListener(OnBuyButtonClick);
            equipButton.onClick.AddListener(OnEquipButtonClick);

            InstantiatePreview();

            var local = rect.localPosition;
            local.z = 0f;
            rect.localPosition = local;

            UpdateButton();
        }

        public void UpdateButton()
        {
            if (isLocked)
            {
                lockedObject.SetActive(true);
                ownedObject.SetActive(false);

                var price = skinData.Price;
                priceText.text = price.ToString();

                if (goldStorage.HasRequiredAmount(price))
                {
                    buyButton.interactable = true;
                }
                else
                {
                    buyButton.interactable = false;
                }
            }
            else
            {
                lockedObject.SetActive(false);
                ownedObject.SetActive(true);

                if (equipped)
                {
                    equippedButton.gameObject.SetActive(true);
                    equipButton.gameObject.SetActive(false);
                }
                else
                {
                    equippedButton.gameObject.SetActive(false);
                    equipButton.gameObject.SetActive(true);
                }
            }
        }

        void InstantiatePreview()
        {
            previewTransform.DestroyAllChildren();

            var skinPrefab = skinData.SkinPrefab;

            var previewInstance = ObjectInstantiator.InstantiatePrefab(skinPrefab, previewTransform);
            previewInstance.transform.ResetAllLocalParameters();

            var materialController = previewInstance.GetComponentInChildren<ISkinMaterialController>();
            materialController.SetNewMaterial(previewMaterial);
        }

        void SetIsLocked(bool value)
        {
            if (isLocked == value) return;

            isLocked = value;
            UpdateButton();
        }

        void OnBuyButtonClick()
        {
            var price = skinData.Price;
            if (goldStorage.HasRequiredAmount(price))
            {
                goldStorage.Reduce(price);
                skinProvider.SetOwnedSkin(dataRuntime);

                SetIsLocked(false);
                SetIsEquipped(true);
            }

            UpdateButton();
        }

        public void SetIsEquipped(bool value)
        {
            if (equipped == value) return;

            skinProvider.SetEquippedSkin(dataRuntime, value);
            equipped = value;

            if (equipped)
            {
                manager.UnequipAllExcept(this);
            }

            UpdateButton();
        }

        void OnEquipButtonClick()
        {
            SetIsEquipped(true);
        }
    }
}