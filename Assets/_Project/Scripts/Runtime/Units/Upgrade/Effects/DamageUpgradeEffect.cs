using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface IDamageGradeable : IGradeable
    {
        void UpdateDamageModifier(float modifier);
    }

    [CreateAssetMenu(fileName = "DamageUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/DamageUpgradeEffect")]
    public class DamageUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            if (unit.TryGetComponentAsInterface<IAttacker>(out var component))
            {
                if (component is IDamageGradeable damageGradeable)
                {
                    var value = GetFullValue(lvl) * valueModifier;
                    damageGradeable.UpdateDamageModifier(value);
                }
                else
                {
                    DebugHelper.LogWarning("Component does not implement IDamageGradeable");
                }
            }
        }
    }
}