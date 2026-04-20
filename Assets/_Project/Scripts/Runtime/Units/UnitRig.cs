using System;
using System.Collections.Generic;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public class UnitRig : MonoBehaviour, IManagedComponent
    {
        List<RigComponent> components = new List<RigComponent>();

        public bool TryGetRigComponent<T>(out T component) where T : RigComponent
        {
            component = components.Find(c => c is T) as T;
            return component != null;
        }

        public bool TryGetComponentAsInterface<T>(out T component) where T : IRigInterface
        {
            var intfc = components.Find(c => c is T);
            if (intfc is T rigInterface)
            {
                component = rigInterface;
                return true;
            }

            component = default;
            return false;
        }

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