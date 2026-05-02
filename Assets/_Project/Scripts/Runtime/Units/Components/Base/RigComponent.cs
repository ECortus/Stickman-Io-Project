using PurrNet;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface IRigInterface
    {
        
    }

    public abstract class RigComponent : NetworkIdentity
    {
        protected UnitRig BaseRig { get; private set; }

        protected T ConvertRig<T>() where T : UnitRig => BaseRig as T;

        public void Initialize(UnitRig rig)
        {
            BaseRig = rig;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            
        }
        
        public void OnRigDestroy()
        {
            OnDestroyed();
            /* Despawn(); */
        }
        
        protected virtual void OnDestroyed()
        {
            
        }
    }
}