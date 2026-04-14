using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class RollAnimationStateBehaviour : AnimationStateBehaviour
    {
        IMovement movement;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            movement ??= animator.GetComponentInParent<IMovement>();
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        protected override void SetOn()
        {
            movement.SetTemporalyDisabled(true);
            base.SetOn();
        }

        protected override void SetOff()
        {
            movement.SetTemporalyDisabled(false);
            base.SetOff();
        }
    }
}