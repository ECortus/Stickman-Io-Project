using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanIo.Runtime.UI
{
    public class USkinButton : MonoBehaviour
    {
        [SerializeField] private bool isDefaultOpened = false;

        [Space(5)]
        [SerializeField] private RectTransform previewTransform;

        [Space(5)]
        [SerializeField] private GameObject lockedObject;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text priceText;
    }
}