using System;
using StickmanIo.Runtime.Player.Data;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IMovement
    {
        public float Speed { get; }
    }
    
    public class PlayerMovement : RigComponent, IMovement
    {
        Vector3 lastPosition;
        Vector3 currentPosition;

        Vector3 moveDirection;

        float speed;

        PlayerData data;
        ICamera cam;

        Rigidbody rb;
        
        public float Speed => speed;
        
        protected override void OnInitialize()
        {
            data = Data;
            cam = Rig.Camera;
            
            rb = GetComponent<Rigidbody>();
            rb.mass = data.Mass;
            
            var inputEvents = Rig.InputEvents;
            inputEvents.OnMoveAction += UpdateMoveDirection;
            inputEvents.OnJumpTriggered += Jump;
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
            WriteCurrentPosition();
            CalculateSpeed();

            var delta = Time.deltaTime;
            UpdateRotation(delta);
            
            UpdateMove();
            
            WriteLastPosition();
        }

        void UpdateMove()
        {
            Vector3 velocity;
            if (moveDirection != Vector3.zero)
            {
                velocity = moveDirection * data.Speed;
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

                var lerp = Quaternion.Lerp(rb.rotation, rotation, data.RotationSpeed * delta);
                rb.MoveRotation(lerp);
            }
        }

        void Jump()
        {
            rb.AddForce(Vector3.up * data.JumpForce, ForceMode.Impulse);
        }

        void WriteLastPosition()
        {
            lastPosition = currentPosition;
        }
        
        void WriteCurrentPosition()
        {
            currentPosition = transform.position;
        }
        
        void CalculateSpeed()
        {
            speed = (currentPosition - lastPosition).sqrMagnitude;
        }
    }
}