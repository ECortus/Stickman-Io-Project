using System;
using GameDevUtils.Runtime;
using PurrNet;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using UnityEngine;
using UnityEngine.Rendering;

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

        [SerializeField] SyncVar<float> currentHealthVar = new SyncVar<float>(0f);
        [SerializeField] SyncVar<float> maxHealthVar = new SyncVar<float>(50f);

        public float CurrentHealth => currentHealthVar.value;
        public float MaxHealth => maxHealthVar.value;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            settings = Data.Settings;

            UpdateMaxHealth();
            Refill();
        }

        protected override void OnDestroyed()
        {

        }

        public void Refill()
        {
            var value = MaxHealth;
            SetCurrentHealth(value);
        }

        public void Heal(float amount)
        {
            var value = CurrentHealth + amount;
            SetCurrentHealth(value);
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
            var value = CurrentHealth - damage;
            SetCurrentHealth(value);

            rig = Rig;

            if (CurrentHealth <= 0f)
            {
                isKilled = true;
                OnDeath();
            }
            else
            {
                isKilled = false;
            }
        }

        float ClampHealth(float value)
        {
            if (CurrentHealth > MaxHealth)
            {
                value = maxHealthVar.value;
            }
            else if (CurrentHealth < 0f)
            {
                value = 0f;
            }

            return value;
        }

        void UpdateMaxHealth()
        {
            var value = settings.BaseMaxHealth * (1f + upgradeableMaxHealthModifierVar.value);
            SetMaxHealth(value);
        }

        [ServerRpc]
        void SetCurrentHealth(float value)
        {
            var clamp = ClampHealth(value);
            currentHealthVar.value = clamp;
        }

        [ServerRpc]
        void SetMaxHealth(float value)
        {
            var clamp = ClampHealth(value);
            maxHealthVar.value = clamp;
        }

        void OnDeath()
        {
            OnDied?.Invoke();

            gameObject.SetActive(false);
            /* Despawn(); */

            /* ObjectHelper.Destroy(this.gameObject); */
        }

        public event Action OnDied;

        SyncVar<float> upgradeableMaxHealthModifierVar = new SyncVar<float>(0f);

        public void UpdateHealthModifier(float modifier)
        {
            SetHealthModifier(modifier);

            UpdateMaxHealth();
            Refill();
        }

        [ServerRpc]
        void SetHealthModifier(float modifier)
        {
            upgradeableMaxHealthModifierVar.value = modifier;
        }
    }
}