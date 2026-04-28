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
            var header = GetComponentInParent<PlayerHeader>();
            header.OnRigInitialize.AddListener(() => health = header.Rig.Health);

            base.OnStart();
        }

        protected override float GetSliderValue()
        {
            if (health == null)
            {
                return 0f;
            }

            var currentHealth = health.CurrentHealth;
            var maxHealth = health.MaxHealth;

            return currentHealth / maxHealth;
        }

        protected override string GetLabelText()
        {
            if (health == null)
            {
                return null;
            }

            var currentHealth = Mathf.RoundToInt(health.CurrentHealth);
            var maxHealth = Mathf.RoundToInt(health.MaxHealth);

            return $"{currentHealth}/{maxHealth}";
        }
    }
}