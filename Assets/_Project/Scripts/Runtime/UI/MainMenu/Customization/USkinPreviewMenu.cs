using StickmanIo.Runtime.UI.ColorPicker;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class USkinPreviewMenu : MonoBehaviour
    {
        [SerializeField] private USkinColorPicker colorPicker;
        [SerializeField] private Material previewMaterial;

        [Space(5)]
        [SerializeField] private Color defaultColor = Color.red;

        void Start()
        {
            colorPicker.SetColor(defaultColor);
            previewMaterial.SetColor("_BaseColor", colorPicker.PickColor());
        }

        void Update()
        {
            previewMaterial.SetColor("_BaseColor", colorPicker.PickColor());
        }
    }
}