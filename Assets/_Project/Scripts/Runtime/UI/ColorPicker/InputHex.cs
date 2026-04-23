#region Includes
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.ColorPicker
{
    public class InputHex : MonoBehaviour
    {
        #region Variables

        InputField _inputLegacy;
        TMP_InputField _input;

        public delegate void OnValueChanged(InputHex sender, Color color);
        public OnValueChanged ValueChanged;

        string Text
        {
            get 
            {
                if (_inputLegacy != null)
                {
                    return _inputLegacy.text;
                }

                if (_input != null)
                {
                    return _input.text;
                }

                return string.Empty;
            }
            set 
            { 
                if (_inputLegacy != null)
                {
                    _inputLegacy.text = value; 
                }

                if (_input != null)
                {
                    _input.text = value; 
                }
            }
        }

        public Color Value
        {
            get
            {
                ColorUtility.TryParseHtmlString(string.Format("#{0}", Text), out Color color);
                return color;
            }
            set
            {
                Text = ColorUtility.ToHtmlStringRGBA(value);
            }
        }

        #endregion

        private void Start()
        {
            _inputLegacy = GetComponentInChildren<InputField>();
            _input = GetComponentInChildren<TMP_InputField>();

            if (_inputLegacy != null)
            {
                _inputLegacy.onEndEdit.AddListener(Input_EndEdit);
            }
            
            if (_input != null)
            {
                _input.onEndEdit.AddListener(Input_EndEdit);
            }
        }

        private void Input_EndEdit(string arg0)
        {
            if (string.IsNullOrEmpty(arg0)) { return; }
            ValueChanged?.Invoke(this, Value);
        }
    }
}