using System;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;
using PurrNet;

namespace StickmanIo.Runtime.Player
{
    public interface IPlayerGroundCheck
    {
        public bool IsOnGround { get; }
    }
    
    public class PlayerGroundCheckController : NetworkIdentity, IPlayerGroundCheck
    {
        [SerializeField, NonSerialized] SyncVar<bool> isOnGroundVar = new SyncVar<bool>(false, ownerAuth: true);
        [SerializeField] LayerMask groundLayer;
        
        public bool IsOnGround => isOnGroundVar.value;

        protected override void OnSpawned()
        {
            base.OnSpawned();

            if (!isOwner)
            {
                enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                SetIsOnGround(true);
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                SetIsOnGround(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                SetIsOnGround(false);
            }
        }

        void SetIsOnGround(bool value)
        {
            if (!isOwner)
            {
                return;
            }

            isOnGroundVar.value = value;
        }
    }
}