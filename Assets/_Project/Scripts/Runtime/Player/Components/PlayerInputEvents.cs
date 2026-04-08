using System;
using StickmanIo.Runtime.Input;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IInputEvents
    {
        event Action<Vector2> OnLookAction;
        event Action<Vector2> OnMoveAction;
        
        event Action OnJumpTriggered;
    }
    
    public class PlayerInputEvents : RigComponent, IInputEvents
    {
        StickmanInputActions inputActions;
        StickmanInputActions.PlayerActions playerActions;
        
        IMovement movement;
        IPlayerGroundCheck groundCheck;
        
        protected override void OnInitialize()
        {
            inputActions = new StickmanInputActions();
            playerActions = inputActions.Player;
            
            playerActions.Enable();
            
            movement = Rig.Movement;
            groundCheck = Rig.GroundCheck;
        }
        
        protected override void OnDestroyed()
        {
            playerActions.Disable();
        }

        private void LateUpdate()
        {
            OnLookUpdate();
            OnMoveUpdate();

            CheckJumpTriggered();
        }

        void OnMoveUpdate()
        {
            var move = playerActions.Move.ReadValue<Vector2>();
            if (move.sqrMagnitude > 0)
            {
                OnMoveAction?.Invoke(move);
            }
            else
            {
                OnMoveAction?.Invoke(Vector2.zero);
            }
        }
        
        public event Action<Vector2> OnMoveAction;
        
        void OnLookUpdate()
        {
            var look = playerActions.Look.ReadValue<Vector2>();
            if (look.sqrMagnitude > 0)
            {
                OnLookAction?.Invoke(look);
            }
            else
            {
                OnLookAction?.Invoke(Vector2.zero);
            }
        }
        
        public event Action<Vector2> OnLookAction;

        void CheckJumpTriggered()
        {
            if (movement.IsRolling)
            {
                return;
            }
            
            if (!groundCheck.IsOnGround)
            {
                return;
            }
            
            var jump = playerActions.Jump.WasPerformedThisFrame();
            if (jump)
            {
                OnJumpTriggered?.Invoke();
            }
        }
        
        public event Action OnJumpTriggered;
    }
}