using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [CreateAssetMenu(fileName = "Upgrade00Data", menuName = "StickmanIo/Upgrades/Data", order = 0)]
    public class UpgradeData : ScriptableObject
    {
        public string id;

        [Space(5)]
        public string title;
        public Sprite icon;

        [Space(10)]
        public UpgradeEffect effect;

        [Range(1f, 5f)]
        public float effectsScaleModifier = 1f;

        //TODO: Action of upgrade, via scriptableobject or choosing script
    }
}