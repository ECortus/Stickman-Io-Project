using System;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IAttacker
    {
        bool IsAttacking { get; }
        event Action<int> OnAttackStarted;
    }

    public class PlayerAttacker : RigComponent, IAttacker
    {
        bool isAttacking;

        bool inputReaded;
        float lastInputTime;

        IMovement movement;
        IPlayerWeaponController weaponController;

        public bool IsAttacking => isAttacking;

        protected override void OnInitialize()
        {
            movement = Rig.Movement;
            movement.OnJump += ResetAttack;

            weaponController = GetComponentInChildren<IPlayerWeaponController>();

            var inputEvents = Rig.InputEvents;
            inputEvents.OnAttackAction += TryAttack;
        }

        protected override void OnDestroyed()
        {

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

            var baseDamage = settings.AttackBaseDamage;

            float maxDelayBetweenInputs = settings.AttackInputsDelay;
            while (Time.time - lastInputTime < maxDelayBetweenInputs)
            {
                inputReaded = true;

                if (attackIndex > maxAttacks)
                {
                    break;
                }

                var currentAttack = attacks[attackIndex - 1];
                var damageMod = currentAttack.DamageModificator;

                AttackStart(attackIndex);

                PreAttack(baseDamage, damageMod);

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

        void PreAttack(float baseDamage, float damageMod)
        {
            isAttacking = true;
            weaponController.SetUpdatedDamage(baseDamage, damageMod);

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
    }
}