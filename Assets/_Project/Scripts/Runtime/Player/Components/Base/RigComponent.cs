using StickmanIo.Runtime.Player.Data;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public abstract class RigComponent : MonoBehaviour
    {
        protected PlayerRig Rig { get; private set; }
        protected PlayerData Data { get; private set; }

        public void Initialize(PlayerRig rig, PlayerData data)
        {
            Rig = rig;
            Data = data;
            
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            
        }
        
        public void OnRigDestroy()
        {
            OnDestroyed();
        }
        
        protected virtual void OnDestroyed()
        {
            
        }
    }
}