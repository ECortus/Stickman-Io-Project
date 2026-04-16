using System;
using System.Collections.Generic;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRig : MonoBehaviour
    {
        PlayerHeader header;

        PlayerHealth health;
        PlayerAttacker attacker;
        
        PlayerCamera cam;
        PlayerMovement movement;

        PlayerInputEvents inputEvents;
        
        IPlayerGroundCheck groundCheck;

        List<RigComponent> components = new List<RigComponent>();

        public bool IsOwner => header.IsOwner;
        
        public IHealth Health => health;
        public IAttacker Attacker => attacker;
        
        public ICamera Camera => cam;
        public IMovement Movement => movement;
        
        public IPlayerGroundCheck GroundCheck => groundCheck;
        
        public IInputEvents InputEvents => inputEvents;
        
        public void Initialize(PlayerHeader hdr)
        {
            header = hdr;
            InitializeComponents();
        }
        
        public void OnDestroy()
        {
            DestroyComponents();
        }
        
        void InitializeComponents()
        {
            health = AddComponent<PlayerHealth>();
            attacker = AddComponent<PlayerAttacker>();
            
            cam = AddComponent<PlayerCamera>();
            movement = AddComponent<PlayerMovement>();
            
            inputEvents = AddComponent<PlayerInputEvents>();

            groundCheck = GetComponentInChildren<IPlayerGroundCheck>();
            
            OnAllComponentsAdded?.Invoke();
            OnAllComponentsAdded = null;
        }
        
        T AddComponent<T>() where T : RigComponent
        {
            var component = gameObject.AddComponent<T>();
            var data = header.Data;
            
            components.Add(component);

            OnAllComponentsAdded += () =>
            {
                component.Initialize(this, data);
            };
            
            return component;
        }
        
        event Action OnAllComponentsAdded;
        
        void DestroyComponents()
        {
            foreach (var c in components)
            {
                c.OnRigDestroy();
            }
            
            components.Clear();
        }
    }
}