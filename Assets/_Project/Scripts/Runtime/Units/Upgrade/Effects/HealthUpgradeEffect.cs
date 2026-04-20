using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [CreateAssetMenu(fileName = "HealthUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/HealthUpgradeEffect")]
    public class HealthUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            throw new System.NotImplementedException();
        }
    }
}