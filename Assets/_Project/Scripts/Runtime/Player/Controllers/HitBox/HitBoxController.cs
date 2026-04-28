using System;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;
using PurrNet;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBox
    {
        GameObject gameObject { get; }

        void SetHitBoxActive(bool active);
    }

    public abstract class HitBoxController : NetworkIdentity, IHitBox
    {
        [SerializeField] private LayerMask hitBoxLayerMask = ~0;

        Collider[] hitBoxColliders;

        protected bool IsInitialized { get; private set; } = false;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            GetAllColliders();

            OnInitialize();
            IsInitialized = true;

            PostInitialize();
        }

        protected virtual void OnInitialize()
        {
            
        }

        protected virtual void PostInitialize()
        {
            
        }

        void GetAllColliders()
        {
            if (IsInitialized)
            {
                return;
            }
            
            hitBoxColliders = GetComponentsInChildren<Collider>();
        }

        public void SetHitBoxActive(bool active)
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            for (int i = 0; i < hitBoxColliders.Length; i++)
            {
                hitBoxColliders[i].enabled = active;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.IsSameMask(hitBoxLayerMask))
            {
                var hitBox = other.GetComponentInParent<IHitBox>();
                if (hitBox != null)
                {
                    OnHitBoxTriggered(hitBox);
                }
            }
        }

        protected virtual void OnHitBoxTriggered(IHitBox hitBox)
        {
            
        }
    }
}