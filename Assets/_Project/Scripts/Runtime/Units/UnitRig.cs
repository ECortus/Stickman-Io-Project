using System;
using System.Collections.Generic;
using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public class UnitRig : MonoBehaviour, IManagedComponent
    {
        List<RigComponent> components = new List<RigComponent>();

        UnitsManager unitsManager;

        public virtual void Initialize()
        {
            unitsManager = UnitsManager.GetInstance;

            InitializeComponents();

            OnAllComponentsAdded?.Invoke();
            OnAllComponentsAdded = null;

            unitsManager.Register(this);
        }

        public virtual void OnDestroy()
        {
            unitsManager.Unregister(this);
            
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