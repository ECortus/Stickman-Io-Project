using System;
using StickmanIo.Runtime.Input;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IInputEvents : IRigInterface
    {
        event Action<Vector2> OnLookAction;
        event Action<Vector2> OnMoveAction;

        event Action OnJumpTriggered;

        event Action OnAttackAction;
    }

    public class PlayerInputEvents : PlayerRigComponent, IInputEvents
    {
        StickmanInputActions inputActions;
        StickmanInputActions.PlayerActions playerActions;

        IMovement movement;
        IAttacker attacker;

        IPlayerGroundCheck groundCheck;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!Rig.IsOwner)
            {
                enabled = false;
                return;
            }

            inputActions = new StickmanInputActions();
            playerActions = inputActions.Player;

            playerActions.Enable();

            movement = Rig.Movement;
            attacker = Rig.Attacker;

            groundCheck = Rig.GroundCheck;
        }

        protected override void OnDestroyed()
        {
            if (!Rig.IsOwner)
            {
                return;
            }

            playerActions.Disable();
        }

        private void LateUpdate()
        {
            OnLookUpdate();
            OnMoveUpdate();

            OnJumpUpdate();
            OnAttackUpdate();
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

        void OnJumpUpdate()
        {
            if (movement.IsDisabled)
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

        void OnAttackUpdate()
        {
            if (movement.IsDisabled)
            {
                return;
            }

            if (!groundCheck.IsOnGround)
            {
                return;
            }

            var attack = playerActions.Attack.WasPerformedThisFrame();
            if (attack)
            {
                OnAttackAction?.Invoke();
            }
        }

        public event Action OnAttackAction;
    }
}