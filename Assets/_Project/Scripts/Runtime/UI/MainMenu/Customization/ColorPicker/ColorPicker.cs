#if UNITY_EDITOR
using UnityEditor;
#endif

#region includes
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StickmanIo.Runtime.MainMenu;
#endregion

namespace StickmanIo.Runtime.UI.ColorPicker
{
    public class ColorPicker : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private HsbPicker _hsbPicker;
        [SerializeField] private Image _colorResult;
        [SerializeField] private InputHex _inputHex;

        [Header("Events")]
        public UnityEvent<Color> OnChanged;
        public UnityEvent<Color> OnSubmit;
        public UnityEvent OnCancel;

        private InputColorChannels _inputRgb;
        private Color _currentColor = Color.white;
        private Texture2D _screenTexture;

        PlayerSkinProvider playerSkinProvider;

        #endregion

        public Color CurrentColor => _currentColor;

        public void SetColor(Color color)
        {
            UpdateColor(color);
        }

        private void Awake()
        {
            _inputRgb = GetComponent<InputColorChannels>();

            playerSkinProvider = PlayerSkinProvider.GetInstance;

            playerSkinProvider.OnColorDeserialized += SetColor;
            OnChanged.AddListener(OnColorChanged);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (_inputRgb == null) { throw new System.Exception("Missing InputColorChannels"); }
#endif
        }

        void OnColorChanged(Color color)
        {
            playerSkinProvider.OnColorChanged(color);
        }

        private void Start()
        {
            _hsbPicker.ValueChanged = HsbPicker_ValueChanged;
            _inputRgb.ValueChanged = InputColorChannels_RGB_ValueChanged;
            _inputHex.ValueChanged = InputHex_ValueChanged;

            enabled = false;

            Open(playerSkinProvider.GetCurrentColor());
        }
        private void Update()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var color = _screenTexture.GetPixel((int)mousePosition.x, (int)mousePosition.y);

            UpdateColor(color);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Destroy(_screenTexture);
                enabled = false;
            }
        }

        public void Open()
        {
            Open(_currentColor);
        }

        public void Open(Color color)
        {
            Enable(true);
            UpdateColor(color);
        }

        private void HsbPicker_ValueChanged(HsbPicker sender, float hue, float saturation, float brightness)
        {
            var color = Color.HSVToRGB(hue, saturation, brightness);

            SetCurrentColor(color);

            SetRgbChannels(color);
            SetHexValue(color);
        }
        private void InputColorChannels_RGB_ValueChanged(InputColorChannels sender, Color color)
        {
            SetCurrentColor(color);

            SetHexValue(color);
            SetHsb(color);
        }
        private void InputHex_ValueChanged(InputHex sender, Color color)
        {
            SetCurrentColor(color);

            SetRgbChannels(color);
            SetHsb(color);
        }

        private void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
        private void UpdateColor(Color color)
        {
            SetCurrentColor(color);

            SetRgbChannels(color);
            SetHexValue(color);
            SetHsb(color);
        }

        private void SetCurrentColor(Color color)
        {
            var previousColor = new Color(_currentColor.r, _currentColor.g, _currentColor.b, _currentColor.a);

            _currentColor = color;
            _colorResult.color = _currentColor;

            if (previousColor != _currentColor)
            {
                OnChanged?.Invoke(_currentColor);
            }
        }

        private void SetRgbChannels(Color color)
        {
            _inputRgb.SetValues(new float[] { color.r, color.g, color.b });
        }

        private void SetHexValue(Color color)
        {
            _inputHex.Value = color;
        }

        private void SetHsb(Color color)
        {
            _hsbPicker.SetColor(color);
        }

        private IEnumerator EnableScreenPicker_Coroutine()
        {
            yield return new WaitForEndOfFrame();

            _screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            _screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            _screenTexture.Apply();

            enabled = true;
        }

        public void OnColorPicker()
        {
            StartCoroutine(EnableScreenPicker_Coroutine());
        }

        public void Apply()
        {
            OnSubmit?.Invoke(_currentColor);
            Enable(false);
        }

        public void Cancel()
        {
            OnCancel?.Invoke();
            Enable(false);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ColorPicker))]
        public class ColorPickerEditor : Editor
        {
            #region Variables

            private ColorPicker _target;
            private Color _color;

            #endregion

            private void OnEnable()
            {
                _target = (ColorPicker)target;
            }
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor");

                if (GUILayout.Button("Open"))
                {
                    _target.Open();
                }

                EditorGUILayout.BeginHorizontal();
                _color = EditorGUILayout.ColorField(_color);
                if (GUILayout.Button("Open with Color"))
                {
                    _target.Open(_color);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
#endif
    }
}