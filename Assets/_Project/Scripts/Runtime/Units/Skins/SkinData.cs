using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [System.Serializable]
    public class SkinData
    {
        [SerializeField] private string id;
        [SerializeField] private GameObject skinPrefab;

        [Space(5)]
        [SerializeField] private GameObject previewPrefab;
        [SerializeField] private int price = 0;

        public string Id => id;
        public GameObject SkinPrefab => skinPrefab;

        public GameObject PreviewPrefab => previewPrefab;
        public int Price => price;
    }
}