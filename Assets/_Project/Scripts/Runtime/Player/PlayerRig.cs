using System;
using System.Collections.Generic;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRig : MonoBehaviour
    {
        PlayerHeader header;

        PlayerHealth health;
        
        PlayerCamera cam;
        PlayerMovement movement;

        PlayerInputEvents inputEvents;

        List<RigComponent> components = new List<RigComponent>();
        
        public IHealth Health => health;
        
        public ICamera Camera => cam;
        public IMovement Movement => movement;
        
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
            
            cam = AddComponent<PlayerCamera>();
            movement = AddComponent<PlayerMovement>();
            
            inputEvents = AddComponent<PlayerInputEvents>();
            
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