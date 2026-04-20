using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface IHealthGradeable : IGradeable
    {
        void UpdateHealthModifier(float modifier);
    }

    [CreateAssetMenu(fileName = "HealthUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/HealthUpgradeEffect")]
    public class HealthUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            if (unit.TryGetComponentAsInterface<IHealth>(out var component))
            {
                if (component is IHealthGradeable healthGradeable)
                {
                    var value = GetFullValue(lvl) * valueModifier;
                    healthGradeable.UpdateHealthModifier(value);
                }
                else
                {
                    DebugHelper.LogWarning("Component does not implement IHealthGradeable");
                }
            }
        }
    }
}