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
        [SerializeField] private UpgradeEffect effect;

        [Range(1f, 5f)]
        [SerializeField] private float effectsScaleModifier = 1f;

        public void ApplyEffect(UnitRig unit, int lvl)
        {
            effect.ApplyEffect(unit, lvl, effectsScaleModifier);
        }
    }
}