using System;
using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        readonly int SpeedHash = Animator.StringToHash("Speed");
        readonly int JumpHash = Animator.StringToHash("Jump");
        readonly int OnGroundHash = Animator.StringToHash("OnGround");

        readonly int NonAttackHash = Animator.StringToHash("NonAttack");
        readonly int AttackOneHash = Animator.StringToHash("Attack1");
        readonly int AttackTwoHash = Animator.StringToHash("Attack2");
        readonly int AttackThreeHash = Animator.StringToHash("Attack3");

        [SerializeField] private Rigidbody parentBody;

        Animator animator;

        PlayerHeader header;
        PlayerRig rig;

        IMovement movement;
        IAttacker attacker;
        IPlayerGroundCheck groundCheck;

        void Start()
        {
            animator = GetComponent<Animator>();

            header = GetComponentInParent<PlayerHeader>();
            if (!header)
            {
                DebugHelper.LogWarning($"AnimationController on {gameObject.name} requires a Header in its parents.");
                return;
            }

            header.OnRigInitialize.AddListener(OnRigInitialize);
        }

        void OnRigInitialize()
        {
            rig = GetComponentInParent<PlayerRig>();

            movement = rig.Movement;
            attacker = rig.Attacker;
            groundCheck = rig.GroundCheck;

            movement.OnJump += TriggerJump;
            attacker.OnAttackStarted += OnAttack;
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
            /* var speed = movement.ActualSpeed; */
            var speed = movement.InputSpeed;
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

        void OnAttack(int attackIndex)
        {
            bool isAttacking = attackIndex > 0;
            if (!isAttacking)
            {
                animator.SetTrigger(NonAttackHash);
            }
            else
            {
                switch (attackIndex)
                {
                    case 1:
                        animator.SetTrigger(AttackOneHash);
                        break;
                    case 2:
                        animator.SetTrigger(AttackTwoHash);
                        break;
                    case 3:
                        animator.SetTrigger(AttackThreeHash);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attackIndex), $"Invalid attack index: {attackIndex}");
                }
            }
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