using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class RollAnimationStateBehaviour : StateMachineBehaviour
    {
        readonly float rollDuration = 0.55f;

        float timer;
        
        IMovement movement;

        bool isEnded;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            movement ??= animator.GetComponentInParent<IMovement>();
            movement.SetRolling(true);
            
            timer = rollDuration;
            isEnded = false;
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnded)
            {
                return;
            }
            
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                StopRolling();
            }
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnded)
            {
                return;
            }
            
            StopRolling();
        }
        
        void StopRolling()
        {
            movement.SetRolling(false);
            isEnded = true;
        }
    }
}