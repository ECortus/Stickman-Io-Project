using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public abstract class AnimationStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] float prepareDuration = 0.05f;
        [SerializeField] float duration = 0.5f;

        float timer;

        bool isSetOff = true;
        bool isEnded = true;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StartAnimation();
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
            else if (timer >= duration)
            {
                StopAnimation();
            }
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnded)
            {
                return;
            }
            
            StopAnimation();
        }
        
        void StartAnimation()
        {
            if (prepareDuration <= 0f)
            {
                SetOn();
            }
            
            timer = 0;
            isEnded = false;
        }

        protected virtual void SetOn()
        {
            isSetOff = false;
        }
        
        protected virtual void SetOff()
        {
            isSetOff = true;
        }
        
        void StopAnimation()
        {
            SetOff();
            isEnded = true;
        }
    }
}