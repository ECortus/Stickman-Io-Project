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

        [SerializeField, ReadOnly] private Rigidbody body;

        Animator animator;

        PlayerHeader header;
        PlayerRig rig;

        IHealth health;
        IMovement movement;
        IAttacker attacker;
        IPlayerGroundCheck groundCheck;

        RagdollController ragdoll;

        void Start()
        {
            animator = GetComponent<Animator>();
            ragdoll = GetComponent<RagdollController>();

            body = GetComponentInParent<Rigidbody>();

            header = GetComponentInParent<PlayerHeader>();
            if (!header)
            {
                DebugHelper.LogWarning($"AnimationController on {gameObject.name} requires a Header in its parents.");
                return;
            }

            header.OnRigInitialize.AddListener(OnRigInitialize);

            ragdoll.OffRagdoll();
        }

        void OnRigInitialize()
        {
            rig = GetComponentInParent<PlayerRig>();

            health = rig.Health;
            movement = rig.Movement;
            attacker = rig.Attacker;
            groundCheck = rig.GroundCheck;

            health.OnDied += OnDied;
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
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attackIndex), $"Invalid attack index: {attackIndex}");
                }
            }
        }

        void SyncPositionsWithBody()
        {
            var bodyPosition = body.position;
            var bodyRotation = body.rotation;

            var localPosition = transform.localPosition;
            localPosition = bodyRotation * localPosition;

            bodyPosition += localPosition;

            transform.localPosition = Vector3.zero;
            body.MovePosition(bodyPosition);
        }

        void OnDied()
        {
            transform.SetParent(null);

            ragdoll.SetToRagdoll();

            animator.enabled = false;
            ObjectHelper.Destroy(this);
        }
    }
}