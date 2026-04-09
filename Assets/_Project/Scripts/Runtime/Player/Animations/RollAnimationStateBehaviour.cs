using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class RollAnimationStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] float prepareDuration = 0.05f;
        [SerializeField] float rollDuration = 0.5f;

        float timer;
        
        IMovement movement;

        bool isSetOff = true;
        bool isEnded = true;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            movement ??= animator.GetComponentInParent<IMovement>();
            StartRolling();
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnded)
            {
                return;
            }
            
            timer += Time.deltaTime;

            if (timer >= prepareDuration && isSetOff)
            {
                SetOn();
            }
            else if (timer >= rollDuration)
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
        
        void StartRolling()
        {
            if (prepareDuration <= 0f)
            {
                SetOn();
            }
            
            timer = 0;
            isEnded = false;
        }

        void SetOn()
        {
            movement.SetRolling(true);
            isSetOff = false;
        }
        
        void SetOff()
        {
            movement.SetRolling(false);
            isSetOff = true;
        }
        
        void StopRolling()
        {
            SetOff();
            isEnded = true;
        }
    }
}