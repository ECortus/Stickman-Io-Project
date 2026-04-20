using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [CreateAssetMenu(fileName = "SpeedUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/SpeedUpgradeEffect", order = 0)]
    public class SpeedUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            throw new System.NotImplementedException();
        }
    }
}