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

        [SerializeField] SyncVar<Vector3> moveDirectionVar = new SyncVar<Vector3>(Vector3.zero);

        [SerializeField] SyncVar<float> speedVar = new SyncVar<float>(0f);

        GlobalPlayerSettings settings;
        
        ICamera cam;
        IAttacker attacker;
        IPlayerGroundCheck groundCheck;

        Rigidbody rb;

        float Speed
        {
            get
            {
                var baseSpeed = groundCheck.IsOnGround ? settings.Speed : settings.SpeedInAir;
                baseSpeed *= 1f + upgradeableSpeedModifier;

                return baseSpeed;
            }
        }

        public float InputSpeed
        {
            get
            {
                var velocity = moveDirectionVar.value * Speed;
                return velocity.magnitude;
            }
        }

        public float ActualSpeed => speedVar;

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

        [ServerRpc]
        void UpdateMoveDirection(Vector2 dir)
        {
            if (dir != Vector2.zero)
            {
                moveDirectionVar.value = new Vector3(dir.x, 0, dir.y);
            }
            else
            {
                moveDirectionVar.value = Vector3.zero;
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
            if (moveDirectionVar != Vector3.zero)
            {
                velocity = moveDirectionVar.value * Speed;
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

            var direction = moveDirectionVar.value;
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

        [ServerRpc]
        void CalculateSpeed(float delta)
        {
            if (moveDirectionVar == Vector3.zero)
            {
                speedVar.value = 0f;
                return;
            }

            speedVar.value = (currentPosition - lastPosition).magnitude / delta;
        }

        public bool IsDisabled { get; private set; }
        public void SetTemporalyDisabled(bool state)
        {
            IsDisabled = state;
        }

        public event Action OnJump;

        float upgradeableSpeedModifier = 0f;

        public void UpdateSpeedModifier(float modifier)
        {
            upgradeableSpeedModifier = modifier;
        }
    }
}