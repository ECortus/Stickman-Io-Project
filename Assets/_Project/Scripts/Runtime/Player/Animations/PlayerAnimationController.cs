using System;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        readonly int SpeedHash = Animator.StringToHash("Speed");
        readonly int JumpHash = Animator.StringToHash("Jump");
        readonly int OnGroundHash = Animator.StringToHash("OnGround");

        [SerializeField] private Rigidbody parentBody;
        
        Animator animator;

        PlayerHeader header;
        PlayerRig rig;
        
        IMovement movement;
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
            
            movement = rig.Movement;
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

        private void LateUpdate()
        {
            SyncPositionsWithBody();
        }

        void UpdateMoveSpeed()
        {
            var speed = movement.Speed;
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

        void SyncPositionsWithBody()
        {
            var bodyPosition = parentBody.position;
            var bodyRotation = parentBody.rotation;
            
            var localPosition = transform.localPosition;
            localPosition = bodyRotation * localPosition;
            
            bodyPosition += localPosition;
            
            transform.localPosition = Vector3.zero;
            parentBody.MovePosition(bodyPosition);
        }
    }
}