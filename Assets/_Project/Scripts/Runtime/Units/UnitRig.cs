using System;
using System.Collections.Generic;
using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public class UnitRig : MonoBehaviour, IManagedComponent
    {
        List<RigComponent> components = new List<RigComponent>();

        public virtual void Initialize()
        {
            InitializeComponents();

            OnAllComponentsAdded?.Invoke();
            OnAllComponentsAdded = null;
        }

        public virtual void OnDestroy()
        {
            DestroyComponents();
        }

        protected virtual void InitializeComponents()
        {
            
        }

        protected T AddComponent<T>() where T : RigComponent
        {
            var component = gameObject.AddComponent<T>();
            components.Add(component);

            OnAllComponentsAdded += () =>
            {
                component.Initialize(this);
            };
            
            return component;
        }

        event Action OnAllComponentsAdded;

        protected void DestroyComponents()
        {
            foreach (var c in components)
            {
                c.OnRigDestroy();
            }
            
            components.Clear();
        }
    }
}