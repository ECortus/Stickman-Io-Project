#region Includes
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.ColorPicker
{
    public class InputChannel : MonoBehaviour
    {
        #region Variables

        InputField _inputLegacy;
        TMP_InputField _input;

        public delegate void OnValueChanged(InputChannel sender, float value, int value32);
        public OnValueChanged ValueChanged;

        public float Value
        {
            get { return Value32 / 255f; }
            set { Value32 = Mathf.RoundToInt(value * 255f); }
        }

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

        public int Value32
        {
            get 
            {
                return Mathf.Clamp(int.Parse(Text), 0, 255);
            }
            set 
            { 
                Text = Mathf.Clamp(value, 0, 255).ToString();
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

            ValueChanged?.Invoke(this, Value, Value32);
        }
    }
}