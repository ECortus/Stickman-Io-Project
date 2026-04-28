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
        [SerializeField] bool isOnGround = false;
        [SerializeField] LayerMask groundLayer;
        
        public bool IsOnGround => isOnGround;

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                isOnGround = true;
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                isOnGround = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.IsSameMask(groundLayer))
            {
                isOnGround = false;
            }
        }
    }
}