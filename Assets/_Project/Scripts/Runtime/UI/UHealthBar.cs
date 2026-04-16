using UnityEngine;
using StickmanIo.Runtime.Player;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.UI
{
    public class UHealthBar : UDoubleSliderField
    {
        IHealth health;

        protected override void OnStart()
        {
            health = GetComponentInParent<IHealth>();
            base.OnStart();
        }

        protected override float GetSliderValue()
        {
            var currentHealth = health.CurrentHealth;
            var maxHealth = health.MaxHealth;

            return currentHealth / maxHealth;
        }
    }
}