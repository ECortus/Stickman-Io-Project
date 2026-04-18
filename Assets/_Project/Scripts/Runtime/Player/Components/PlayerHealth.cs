using System;
using GameDevUtils.Runtime;

namespace StickmanIo.Runtime.Player
{
    public interface IHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void Heal(float amount);
        void TakeDamage(float damage, out bool isKilled);

        event Action OnDied;
    }
    
    public class PlayerHealth : RigComponent, IHealth
    {
        float currentHealth;
        float maxHealth;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        protected override void OnInitialize()
        {
            var settings = Data.Settings;
            maxHealth = settings.BaseMaxHealth;

            Heal(maxHealth);
        }
        
        protected override void OnDestroyed()
        {
            
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            ClampHealth();
        }

        public void TakeDamage(float damage, out bool isKilled)
        {
            currentHealth -= damage;
            ClampHealth();

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
    }
}