using System;
using PurrNet;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IMovement : IRigInterface, ISpeedGradeable
    {
        public float InputSpeed { get; }
        public float ActualSpeed { get; }

        public bool IsDisabled { get; }
        public void SetTemporalyDisabled(bool state);

        public event Action OnJump;
    }

    public class PlayerMovement : PlayerRigComponent, IMovement
    {
        Vector3 lastPosition;
        Vector3 currentPosition;

        [SerializeField, NonSerialized] SyncVar<Vector3> moveDirectionVar = new SyncVar<Vector3>(Vector3.zero, ownerAuth: true);

        [SerializeField, NonSerialized] SyncVar<float> calculatedSpeedVar = new SyncVar<float>(0f, ownerAuth: true);

        [SerializeField, NonSerialized] SyncVar<float> upgradeableSpeedModifier = new SyncVar<float>(0f, ownerAuth: true);

        GlobalPlayerSettings settings;
        
        ICamera cam;
        IAttacker attacker;
        IPlayerGroundCheck groundCheck;

        Rigidbody rb;

        Vector3 MoveDirection => moveDirectionVar.value;
        float CalculatedSpeed => calculatedSpeedVar.value;

        float Speed
        {
            get
            {
                var baseSpeed = groundCheck.IsOnGround ? settings.Speed : settings.SpeedInAir;
                baseSpeed *= 1f + upgradeableSpeedModifier.value;

                return baseSpeed;
            }
        }

        public float InputSpeed
        {
            get
            {
                var velocity = MoveDirection * Speed;
                return velocity.magnitude;
            }
        }

        public float ActualSpeed => CalculatedSpeed;

        protected override void OnInitialize()
        {
            base.OnInitialize();
    
            settings = Data.Settings;

            cam = Rig.Camera;
            attacker = Rig.Attacker;
            groundCheck = Rig.GroundCheck;

            rb = GetComponent<Rigidbody>();
            rb.mass = settings.Mass;

            var inputEvents = Rig.InputEvents;
            inputEvents.OnMoveAction += UpdateMoveDirection;
            inputEvents.OnJumpTriggered += Jump;
        }

        protected override void OnDestroyed()
        {

        }

        void UpdateMoveDirection(Vector2 dir)
        {
            if (dir != Vector2.zero)
            {
                SetMoveDirection(new Vector3(dir.x, 0, dir.y));
            }
            else
            {
                SetMoveDirection(Vector3.zero);
            }
        }

        private void Update()
        {
            var delta = Time.deltaTime;

            WriteCurrentPosition();
            CalculateSpeed(delta);

            UpdateRotation(delta);

            if (!IsDisabled)
            {
                UpdateMove();
            }

            WriteLastPosition();
        }

        void UpdateMove()
        {
            Vector3 velocity;
            if (MoveDirection != Vector3.zero)
            {
                velocity = MoveDirection * Speed;
            }
            else
            {
                velocity = Vector3.zero;
            }

            var cameraRotation = Quaternion.Euler(new Vector3(0, cam.RotationHorizontalAngle, 0));
            velocity = cameraRotation * velocity;

            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;
        }

        void UpdateRotation(float delta)
        {
            rb.angularVelocity = Vector3.zero;

            var direction = MoveDirection;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(direction);

                var angles = rotation.eulerAngles;
                angles.y += cam.RotationHorizontalAngle;
                rotation = Quaternion.Euler(angles);

                var lerp = Quaternion.Lerp(rb.rotation, rotation, settings.RotationSpeed * delta);
                rb.MoveRotation(lerp);
            }
        }

        void Jump()
        {
            rb.AddForce(Vector3.up * settings.JumpForce, ForceMode.Impulse);
            OnJump?.Invoke();
        }

        void WriteLastPosition()
        {
            var pos = transform.position;
            pos.y = 0;

            lastPosition = pos;
        }

        void WriteCurrentPosition()
        {
            var pos = transform.position;
            pos.y = 0;

            currentPosition = pos;
        }

        void CalculateSpeed(float delta)
        {
            if (!isOwner)
            {
                return;
            }

            if (MoveDirection == Vector3.zero)
            {
                calculatedSpeedVar.value = 0f;
                return;
            }

            calculatedSpeedVar.value = (currentPosition - lastPosition).magnitude / delta;
        }

        public bool IsDisabled { get; private set; }
        public void SetTemporalyDisabled(bool state)
        {
            IsDisabled = state;
        }

        public event Action OnJump;

        public void UpdateSpeedModifier(float modifier)
        {
            if (!isOwner)
            {
                return;
            }

            upgradeableSpeedModifier.value = modifier;
        }

        void SetMoveDirection(Vector3 value)
        {
            if (!isOwner)
            {
                return;
            }

            moveDirectionVar.value = value;
        }
    }
}