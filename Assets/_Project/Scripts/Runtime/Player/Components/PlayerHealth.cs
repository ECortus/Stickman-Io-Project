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

        [SerializeField, NonSerialized] SyncVar<float> currentHealthVar = new SyncVar<float>(0f, ownerAuth: true);
        [SerializeField, NonSerialized] SyncVar<float> maximumHealthVar = new SyncVar<float>(0f, ownerAuth: true);

        [SerializeField, NonSerialized] SyncVar<float> upgradeableMaxHealthModifierVar = new SyncVar<float>(0f, ownerAuth: true);

        bool isDead = false;

        public float CurrentHealth => currentHealthVar.value;
        public float MaxHealth => maximumHealthVar.value;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            settings = Data.Settings;

            UpdateMaxHealth();
            Resurrect();
        }

        protected override void OnDestroyed()
        {

        }

        public void Resurrect()
        {
            Heal_Internal(MaxHealth);
        }

        public void Heal(float amount)
        {
            Heal_Internal(amount);
        }

        public void UpdateHealthModifier(float modifier)
        {
            SetHealthModifier(modifier);

            UpdateMaxHealth();
            Resurrect();
        }

        public void TakeDamage(float damage)
        {
            TakeDamage_Internal(damage, out _, out _);
        }

        public void TakeDamage(float damage, out bool isKilled)
        {
            TakeDamage_Internal(damage, out isKilled, out _);
        }

        public void TakeDamage(float damage, out bool isKilled, out IPlayerRig rig)
        {
            TakeDamage_Internal(damage, out isKilled, out rig);
        }

        void Heal_Internal(float amount)
        {
            if (isDead & amount > 0)
            {
                isDead = false;
            }

            var value = CurrentHealth + amount;
            SetCurrentHealth(value);
        }

        void TakeDamage_Internal(float damage, out bool isKilled, out IPlayerRig rig)
        {
            if (isDead)
            {
                isKilled = false;
                rig = null;

                return;
            }

            var current = CurrentHealth - damage;
            SetCurrentHealth(current);

            rig = Rig;

            if (current <= 0f)
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
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            else if (value <= 0f)
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

        void SetCurrentHealth(float value)
        {
            if (!isOwner)
            {
                return;
            }

            var clamp = ClampHealth(value);
            currentHealthVar.value = clamp;
        }

        void SetMaxHealth(float value)
        {
            if (!isOwner)
            {
                return;
            }

            maximumHealthVar.value = value;
        }

        void SetHealthModifier(float modifier)
        {
            if (!isOwner)
            {
                return;
            }

            upgradeableMaxHealthModifierVar.value = modifier;
        }

        void OnDeath()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;
            OnDied?.Invoke();

            gameObject.SetActive(false);

            this.Invoke("OnDeathInvoke", 0.25f);
        }

        void OnDeathInvoke()
        {
            Despawn();
            /* ObjectHelper.Destroy(this.gameObject); */
        }

        public event Action OnDied;
    }
}