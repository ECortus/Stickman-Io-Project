using System;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        readonly int SpeedHash = Animator.StringToHash("Speed");
        readonly int JumpHash = Animator.StringToHash("Jump");
        readonly int OnGroundHash = Animator.StringToHash("OnGround");
        
        Animator animator;

        PlayerHeader header;
        PlayerRig rig;
        
        IPlayerGroundCheck groundCheck;
        
        void Start()
        {
            animator = GetComponent<Animator>();
            
            header = GetComponentInParent<PlayerHeader>();
            header.OnRigInitialize.AddListener(OnRigInitialize);
        }

        void OnRigInitialize()
        {
            rig = GetComponentInParent<PlayerRig>();
            groundCheck = rig.GroundCheck;
            
            var inputEvents = rig.InputEvents;
            inputEvents.OnJumpTriggered += TriggerJump;
        }

        private void Update()
        {
            if (!rig)
            {
                return;
            }
            
            UpdateMoveSpeed();
            UpdateOnGround();
        }

        void UpdateMoveSpeed()
        {
            var speed = rig.Movement.Speed;
            SetSpeed(speed);
        }
        
        void UpdateOnGround()
        {
            var onGround = groundCheck.IsOnGround;
            SetOnGround(onGround);
        }

        void SetSpeed(float speed)
        {
            animator.SetFloat(SpeedHash, speed);
        }

        void TriggerJump()
        {
            animator.SetTrigger(JumpHash);
        }
        
        void SetOnGround(bool onGround)
        {
            animator.SetBool(OnGroundHash, onGround);
        }
    }
}