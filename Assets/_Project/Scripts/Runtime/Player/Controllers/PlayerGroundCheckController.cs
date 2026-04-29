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
        [SerializeField] SyncVar<bool> isOnGroundVar = new SyncVar<bool>(false);
        [SerializeField] LayerMask groundLayer;
        
        public bool IsOnGround => isOnGroundVar;

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

        [ServerRpc]
        void SetIsOnGround(bool value)
        {
            isOnGroundVar.value = value;
        }
    }
}