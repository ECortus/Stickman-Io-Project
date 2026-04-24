using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [System.Serializable]
    public class SkinData
    {
        [SerializeField] private string id;
        [SerializeField] private GameObject skinPrefab;

        [Space(5)]
        [SerializeField] private bool isLocked = true;
        [SerializeField] private int price = 0;

        public string Id => id;
        public GameObject SkinPrefab => skinPrefab;

        public bool IsLocked => isLocked;
        public int Price => price;
    }
}