using System;
using GameDevUtils.Runtime;
using PurrNet;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
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

        IPlayerSaveable playerSaveable;

        [SerializeField, NonSerialized] SyncVar<float> currentHealthVar = new SyncVar<float>(0f);
        [SerializeField, NonSerialized] SyncVar<float> maximumHealthVar = new SyncVar<float>(0f, ownerAuth: true);

        [SerializeField, NonSerialized] SyncVar<float> upgradeableMaxHealthModifierVar = new SyncVar<float>(0f, ownerAuth: true);

        ICamera cam;

        bool isDead = false;

        bool IsDead => isDead;

        public float CurrentHealth => currentHealthVar.value;
        public float MaxHealth => maximumHealthVar.value;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            settings = Data.Settings;

            playerSaveable = Rig.Saveable;

            maximumHealthVar.onChanged += OnManHealthChanged;

            if (isOwner)
            {
                cam = Rig.Camera;

                UpdateMaxHealth();
                Resurrect();
            }
        }

        protected override void OnDespawned()
        {
            base.OnDespawned();
            OnDeath();
        }

        protected override void OnDestroyed()
        {
            maximumHealthVar.onChanged -= OnManHealthChanged;
        }

        void OnManHealthChanged(float value)
        {
            UpdateMaxHealth();
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
            if (IsDead & amount > 0)
            {
                SetIsDead(false);
            }

            var value = CurrentHealth + amount;
            value = Mathf.Clamp(value, 0f, MaxHealth);

            Health_Rpc(value);
        }

        [ServerRpc]
        void Health_Rpc(float amount)
        {
            SetCurrentHealth(amount);
        }

        void TakeDamage_Internal(float damage, out bool isKilled, out IPlayerRig rig)
        {
            rig = Rig;

            if (IsDead)
            {
                isKilled = true;
                return;
            }

            var current = CurrentHealth - damage;
            if (current <= 0f)
            {
                isKilled = true;
            }
            else
            {
                isKilled = false;
            }

            TakeDamage_Rpc(damage);
        }

        [ServerRpc]
        void TakeDamage_Rpc(float damage)
        {
            var current = CurrentHealth - damage;
            SetCurrentHealth(current);

            if (isOwner)
            {
                cam.ShakeOnHit();
            }

            if (current <= 0f)
            {
                OnDeath();
            }
        }

        void UpdateMaxHealth()
        {
            var value = settings.BaseMaxHealth * (1f + upgradeableMaxHealthModifierVar.value);
            SetMaxHealth(value);
        }

        void SetCurrentHealth(float value)
        {
            currentHealthVar.value = value;
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
            var networkManager = NetworkManager.main;
            if (!networkManager || networkManager.serverState != PurrNet.Transports.ConnectionState.Connected
            || networkManager.clientState != PurrNet.Transports.ConnectionState.Connected)
            {
                return;
            }

            if (IsDead)
            {
                return;
            }

            PlayersLogger.LogKilled($"Player ID-{Rig.localPlayer.Value.id}");

            SetIsDead(true);
            OnDied?.Invoke();

            playerSaveable.TrySavePrefs(true);

            Despawn();
            ObjectHelper.Destroy(this.gameObject);
        }

        void SetIsDead(bool value)
        {
            isDead = value;
        }

        public event Action OnDied;
    }
}