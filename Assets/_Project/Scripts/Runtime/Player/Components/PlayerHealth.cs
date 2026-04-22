using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;

namespace StickmanIo.Runtime.Player
{
    public interface IHealth : IRigInterface, IHealthGradeable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void Heal(float amount);

        void TakeDamage(float damage);
        void TakeDamage(float damage, out bool isKilled);
        void TakeDamage(float damage, out bool isKilled, out IPlayerRig rig);

        event Action OnDied;
    }

    public class PlayerHealth : PlayerRigComponent, IHealth
    {
        GlobalPlayerSettings settings;

        float currentHealth;
        float maxHealth => settings.BaseMaxHealth * (1f + upgradeableMaxHealthModifier);

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            settings = Data.Settings;
            Refill();
        }

        protected override void OnDestroyed()
        {

        }

        public void Refill()
        {
            currentHealth = maxHealth;
            ClampHealth();
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            ClampHealth();
        }

        public void TakeDamage(float damage)
        {
            TakeDamage(damage, out _, out _);
        }

        public void TakeDamage(float damage, out bool isKilled)
        {
            TakeDamage(damage, out isKilled, out _);
        }

        public void TakeDamage(float damage, out bool isKilled, out IPlayerRig rig)
        {
            currentHealth -= damage;
            ClampHealth();

            rig = Rig;

            if (currentHealth <= 0f)
            {
                isKilled = true;
                OnDeath();
            }
            else
            {
                isKilled = false;
            }
        }

        void ClampHealth()
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth < 0f)
            {
                currentHealth = 0f;
            }
        }

        void OnDeath()
        {
            OnDied?.Invoke();
            ObjectHelper.Destroy(this.gameObject);
        }

        public event Action OnDied;

        float upgradeableMaxHealthModifier = 0f;

        public void UpdateHealthModifier(float modifier)
        {
            upgradeableMaxHealthModifier = modifier;
            Refill();
        }
    }
}