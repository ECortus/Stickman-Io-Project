using System;

namespace StickmanIo.Runtime.Player
{
    public interface IHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void Heal(float amount);
        void TakeDamage(float damage);

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

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            ClampHealth();

            if (currentHealth <= 0f)
            {
                OnDeath();
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
        }

        public event Action OnDied;
    }
}