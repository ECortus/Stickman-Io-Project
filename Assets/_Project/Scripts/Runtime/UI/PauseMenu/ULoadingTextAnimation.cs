using System;
using TMPro;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class ULoadingTextAnimation : MonoBehaviour
    {
        [SerializeField] private float animationSpeed = 1f;
        [SerializeField] private int dotsCount = 6;
        [SerializeField] private string defaultText = "Loading";
        [SerializeField] private TMP_Text loadingText;

        int _currentDotIndex = 0;
        float time = 0;

        void Start()
        {
            loadingText.text = defaultText;
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > 1f / animationSpeed)
            {
                time = 0;
                _currentDotIndex = (_currentDotIndex + 1) % dotsCount;
                loadingText.text = defaultText + new String('.', _currentDotIndex + 1);
            }
        }
    }
}