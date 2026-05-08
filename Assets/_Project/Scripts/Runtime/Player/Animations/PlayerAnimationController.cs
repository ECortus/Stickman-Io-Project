using System;
using GameDevUtils.Runtime;
using PurrNet;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerAnimationController : NetworkIdentity
    {
        readonly int SpeedHash = Animator.StringToHash("Speed");
        readonly int JumpHash = Animator.StringToHash("Jump");
        readonly int OnGroundHash = Animator.StringToHash("OnGround");

        readonly int NonAttackHash = Animator.StringToHash("NonAttack");
        readonly int AttackOneHash = Animator.StringToHash("Attack1");
        readonly int AttackTwoHash = Animator.StringToHash("Attack2");

        [SerializeField, ReadOnly] private Rigidbody body;

        Animator animator;
        NetworkAnimator networkAnimator;

        PlayerHeader header;
        PlayerRig rig;

        IHealth health;
        IMovement movement;
        IAttacker attacker;
        IPlayerGroundCheck groundCheck;

        ISkinMaterialController skinController;

        RagdollController ragdoll;

        protected override void OnSpawned()
        {
            base.OnSpawned();
            Initialize();
        }

        void Initialize()
        {
            header = GetComponentInParent<PlayerHeader>();

            animator = GetComponent<Animator>();
            networkAnimator = GetComponent<NetworkAnimator>();

            ragdoll = GetComponent<RagdollController>();
            ragdoll.OffRagdoll();

            if (!header)
            {
                enabled = false;

                OnDied();
                return;
            }

            skinController = GetComponentInChildren<ISkinMaterialController>();

            body = GetComponentInParent<Rigidbody>();

            header.OnRigInitialize.AddListener(OnRigInitialize);
        }

        void OnRigInitialize()
        {
            rig = header.Rig;

            health = rig.Health;
            movement = rig.Movement;
            attacker = rig.Attacker;
            groundCheck = rig.GroundCheck;

            health.OnHit += OnHit;
            health.OnDied += OnDied;
            movement.OnJump += TriggerJump;
            attacker.OnAttackStarted += OnAttack;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!rig)
            {
                return;
            }

            health.OnHit -= OnHit;
            health.OnDied -= OnDied;
            movement.OnJump -= TriggerJump;
            attacker.OnAttackStarted -= OnAttack;
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
            if (!rig)
            {
                return;
            }

            SyncPositionsWithBody();
        }

        void UpdateMoveSpeed()
        {
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
            networkAnimator.SetFloat(SpeedHash, speed);
        }

        void TriggerJump()
        {
            networkAnimator.SetTrigger(JumpHash);
        }


        void SetOnGround(bool onGround)
        {
            networkAnimator.SetBool(OnGroundHash, onGround);
        }

        void OnAttack(int attackIndex)
        {
            bool isAttacking = attackIndex > 0;
            if (!isAttacking)
            {
                networkAnimator.SetTrigger(NonAttackHash);
            }
            else
            {
                switch (attackIndex)
                {
                    case 1:
                        networkAnimator.SetTrigger(AttackOneHash);
                        break;
                    case 2:
                        networkAnimator.SetTrigger(AttackTwoHash);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attackIndex), $"Invalid attack index: {attackIndex}");
                }
            }
        }

        void OnHit()
        {
            var settings = header.Data.Settings;
            skinController.BlinkAnimation(settings.blickDurationOnHit, settings.blickFrequencyOnHit);
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

            animator.enabled = false;
            networkAnimator.enabled = false;

            this.enabled = false;

            ragdoll.SetToRagdoll();
        }
    }
}