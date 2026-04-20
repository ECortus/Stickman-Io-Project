using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface ISpeedGradeable : IGradeable
    {
        void UpdateSpeedModifier(float modifier);
    }

    [CreateAssetMenu(fileName = "SpeedUpgradeEffect", menuName = "StickmanIo/Upgrades/Effects/SpeedUpgradeEffect", order = 0)]
    public class SpeedUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f)
        {
            if (unit.TryGetComponentAsInterface<IMovement>(out var component))
            {
                if (component is ISpeedGradeable speedGradeable)
                {
                    var value = GetFullValue(lvl) * valueModifier;
                    speedGradeable.UpdateSpeedModifier(value);
                }
                else
                {
                    DebugHelper.LogWarning("Component does not implement ISpeedGradeable");
                }
            }
        }
    }
}