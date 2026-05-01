using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.MainMenu;
using StickmanIo.Runtime.UI.ColorPicker;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class USkinPreviewMenu : MonoBehaviour
    {
        [SerializeField] private USkinColorPicker colorPicker;
        [SerializeField] private Material previewMaterial;

        [Space(5)]
        [SerializeField] private RectTransform previewTransform;

        [Space(5)]
        [SerializeField] private Color defaultColor = Color.red;

        PlayerSkinProvider skinProvider;

        void Start()
        {
            skinProvider = PlayerSkinProvider.GetInstance;
            skinProvider.OnSkinEquipped += UpdateSkinData;

            SetColor(defaultColor);
            UpdateSkinData();
        }

        void Update()
        {
            UpdatePreviewColor();
        }

        void SetColor(Color color)
        {
            colorPicker.SetColor(color);
            UpdatePreviewColor();
        }

        void UpdatePreviewColor()
        {
            previewMaterial.SetColor("_BaseColor", colorPicker.PickColor());
        }

        void UpdateSkinData()
        {
            var runtimeData = skinProvider.GetEquippedSkinData();
            if (runtimeData == null) 
            {
                return;
            }

            var skinPrefab = runtimeData.SkinData.SkinPrefab;
            InstantiatePreview(skinPrefab);
        }

        void InstantiatePreview(GameObject skinPrefab)
        {
            previewTransform.DestroyAllChildren();

            var previewInstance = ObjectInstantiator.InstantiatePrefab(skinPrefab, previewTransform);
            previewInstance.transform.ResetAllLocalParameters();

            var materialController = previewInstance.GetComponentInChildren<ISkinMaterialController>();
            materialController.SetNewMaterialAsNonInstance(previewMaterial);
        }
    }
}