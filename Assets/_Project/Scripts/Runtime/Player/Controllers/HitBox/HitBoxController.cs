using System;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBox
    {
        GameObject gameObject { get; }

        void SetHitBoxActive(bool active);
    }

    public abstract class HitBoxController : MonoBehaviour, IHitBox
    {
        [SerializeField] private LayerMask hitBoxLayerMask = ~0;

        Collider[] hitBoxColliders;

        bool initialized = false;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            GetAllColliders();

            OnInitialize();
            initialized = true;

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
            if (initialized)
            {
                return;
            }
            
            hitBoxColliders = GetComponentsInChildren<Collider>();
        }

        public void SetHitBoxActive(bool active)
        {
            if (!initialized)
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