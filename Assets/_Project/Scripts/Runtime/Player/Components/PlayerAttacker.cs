using System;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IAttacker : IRigInterface, IDamageGradeable
    {
        bool IsAttacking { get; }

        void DamageUnit(IHitBoxReceiver unit);

        event Action<int> OnAttackStarted;
    }

    public class PlayerAttacker : PlayerRigComponent, IAttacker
    {
        bool isAttacking;

        bool inputReaded;
        float lastInputTime;

        float damage = -1f;
        float damageMod = 1f;

        IMovement movement;
        ILevel level;
        IResources resources;

        IPlayerWeaponController weaponController;

        public bool IsAttacking => isAttacking;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            movement = Rig.Movement;
            level = Rig.Level;
            resources = Rig.Resources;

            movement.OnJump += ResetAttack;

            weaponController = GetComponentInChildren<IPlayerWeaponController>();

            var inputEvents = Rig.InputEvents;
            inputEvents.OnAttackAction += TryAttack;
        }

        protected override void OnDestroyed()
        {

        }

        public void DamageUnit(IHitBoxReceiver unit)
        {
            var dmg = damage * damageMod;
            unit.Damage(dmg, out bool isKilled, out IPlayerRig rig);

            if (isKilled)
            {
                resources.AddScore(rig);
                level.AddLevel();
            }
        }

        void TryAttack()
        {
            lastInputTime = Time.time;
            inputReaded = false;

            if (isAttacking)
            {
                return;
            }

            AsyncTaskHelper.CreateTask(async () => await Attack_Process());
        }

        async UniTask Attack_Process()
        {
            var settings = Data.Settings;
            var attacks = settings.AttacksData;

            int attackIndex = 1;
            int maxAttacks = attacks.Length;

            damage = settings.BaseAttackDamage;
            damage *= 1f + upgradeableDamageModifier;

            float maxDelayBetweenInputs = settings.AttackInputsDelay;
            while (true)
            {
                if (Time.time - lastInputTime > maxDelayBetweenInputs)
                {
                    await UniTask.Yield();
                    if (Time.time - lastInputTime > maxDelayBetweenInputs * 2f)
                    {
                        break;
                    }

                    continue;
                }

                inputReaded = true;

                if (attackIndex > maxAttacks)
                {
                    break;
                }

                var currentAttack = attacks[attackIndex - 1];
                damageMod = currentAttack.DamageModificator;

                AttackStart(attackIndex);

                PreAttack();

                var prepareDuration = currentAttack.PrepareDuration;
                await UniTask.Delay(TimeSpan.FromSeconds(prepareDuration));

                OnAttack();

                var attackDuration = currentAttack.AttackDuration;
                await UniTask.Delay(TimeSpan.FromSeconds(attackDuration));

                PostAttack();

                float postTime = 0;
                var postDuration = currentAttack.PostDuration;
                while (postTime < postDuration)
                {
                    if (!inputReaded && Time.time - lastInputTime < maxDelayBetweenInputs)
                    {
                        break;
                    }

                    postTime += Time.deltaTime;
                    await UniTask.Yield();
                }

                attackIndex++;
            }

            ResetAttack();
        }

        void AttackStart(int attackIndex)
        {
            OnAttackStarted?.Invoke(attackIndex);
        }

        void PreAttack()
        {
            isAttacking = true;
            weaponController.SetWeaponActive(true);
        }

        void OnAttack()
        {
            weaponController.SetCollidersActive(true);
        }

        void PostAttack()
        {
            weaponController.SetCollidersActive(false);
        }

        void ResetAttack()
        {
            OnAttackStarted?.Invoke(0);
            isAttacking = false;
        }

        readonly float weaponActiveDuration = 5f;

        void Update()
        {
            if (weaponController.WeaponActive)
            {
                if (Time.time - lastInputTime > weaponActiveDuration)
                {
                    weaponController.SetWeaponActive(false);
                }
            }
        }

        public event Action<int> OnAttackStarted;

        float upgradeableDamageModifier = 0f;
        
        public void UpdateDamageModifier(float modifier)
        {
            upgradeableDamageModifier = modifier;
        }
    }
}