using UnityEngine;

namespace StickmanIo.Runtime.UI.ColorPicker
{
    public class USkinColorPicker : MonoBehaviour
    {
        ColorPicker colorPicker;

        void Awake()
        {
            colorPicker = GetComponentInChildren<ColorPicker>();
        }

        public Color PickColor()
        {
            return colorPicker.CurrentColor;
        }

        public void SetColor(Color color)
        {
            colorPicker.SetColor(color);
        }
    }
}