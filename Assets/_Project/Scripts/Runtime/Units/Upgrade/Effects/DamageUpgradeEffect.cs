using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [CreateAssetMenu(fileName = "DamageUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/DamageUpgradeEffect")]
    public class DamageUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            throw new System.NotImplementedException();
        }
    }
}