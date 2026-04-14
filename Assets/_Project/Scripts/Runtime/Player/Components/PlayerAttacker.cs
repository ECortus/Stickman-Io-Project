using System;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IAttacker
    {
        bool IsAttacking { get; }
        event Action<int> OnAttackAction;
    }
    
    public class PlayerAttacker : RigComponent, IAttacker
    {
        bool isAttacking;

        float lastInputTime;

        public bool IsAttacking => isAttacking;

        protected override void OnInitialize()
        {
            var inputEvents = Rig.InputEvents;
            inputEvents.OnAttackAction += TryAttack;
        }
        
        protected override void OnDestroyed()
        {
            
        }

        void TryAttack()
        {
            lastInputTime = Time.time;

            if (isAttacking)
            {
                return;
            }

            AsyncTaskHelper.CreateTask(async () => await Attack_Process());
        }

        async UniTask Attack_Process()
        {
            isAttacking = true;

            OnAttackAction?.Invoke(1);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            isAttacking = false;
        }

        public event Action<int> OnAttackAction;
    }
}