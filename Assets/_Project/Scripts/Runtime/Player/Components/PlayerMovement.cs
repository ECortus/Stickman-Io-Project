using System;
using StickmanIo.Runtime.Player.Data;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IMovement
    {
        public float InputSpeed { get; }
        public float ActualSpeed { get; }

        public bool IsDisabled { get; }
        public void SetTemporalyDisabled(bool state);

        public event Action OnJump;
    }

    public class PlayerMovement : RigComponent, IMovement
    {
        Vector3 lastPosition;
        Vector3 currentPosition;

        Vector3 moveDirection;

        float speed;

        GlobalPlayerSettings settings;
        ICamera cam;
        IAttacker attacker;

        Rigidbody rb;

        public float InputSpeed
        {
            get
            {
                var velocity = moveDirection * settings.Speed;
                return velocity.magnitude;
            }
        }

        public float ActualSpeed => speed;

        protected override void OnInitialize()
        {
            settings = Data.Settings;

            cam = Rig.Camera;
            attacker = Rig.Attacker;

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
                moveDirection = new Vector3(dir.x, 0, dir.y);
            }
            else
            {
                moveDirection = Vector3.zero;
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
            if (moveDirection != Vector3.zero)
            {
                velocity = moveDirection * settings.Speed;
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

            var direction = moveDirection;
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
            if (moveDirection == Vector3.zero)
            {
                speed = 0f;
                return;
            }

            speed = (currentPosition - lastPosition).magnitude / delta;
        }

        public bool IsDisabled { get; private set; }
        public void SetTemporalyDisabled(bool state)
        {
            IsDisabled = state;
        }

        public event Action OnJump;
    }
}