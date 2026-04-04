using System;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRig : MonoBehaviour
    {
        private PlayerHeader Header { get; set; }
        
        public PlayerHealth Health { get; private set; }
        private PlayerMovement Movement { get; set; }
        
        public PlayerInputEvents InputEvents { get; private set; }
        
        public void Initialize(PlayerHeader hdr)
        {
            Header = hdr;
            InitializeComponents();
        }
        
        void InitializeComponents()
        {
            Health = AddComponent<PlayerHealth>();
            Movement = AddComponent<PlayerMovement>();
            
            OnAllComponentsAdded?.Invoke();
            OnAllComponentsAdded = null;
        }
        
        T AddComponent<T>() where T : RigComponent
        {
            var component = gameObject.AddComponent<T>();
            var data = Header.Data;

            OnAllComponentsAdded += () =>
            {
                component.Initialize(this, data);
            };
            
            return component;
        }
        
        event Action OnAllComponentsAdded;
    }
}