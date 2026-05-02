using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;

namespace StickmanIo.Runtime.Units
{
    public class UnitsManager : AbstractComponentManager<UnitRig, UnitsManager>
    {
        public PlayerRig OwnerRig { get; private set; }

        public event Action OnOwnerRigChanged;

        public override void Register(UnitRig element)
        {
            if (element is PlayerRig playerRig)
            {
                if (playerRig.IsOwner)
                {
                    if (OwnerRig != null)
                    {
                        throw new System.Exception("OwnerRig already set, it can be only ONE owner on scene.");
                    }

                    OwnerRig = playerRig;
                    OnOwnerRigChanged?.Invoke();
                }
            }

            base.Register(element);
        }

        public override void Unregister(UnitRig element)
        {
            if (element is PlayerRig playerRig)
            {
                if (playerRig.IsOwner)
                {
                    if (OwnerRig != playerRig)
                    {
                        throw new System.Exception("OwnerRig can be only ONE owner on scene.");
                    }

                    OwnerRig = null;
                    OnOwnerRigChanged?.Invoke();
                }
            }

            base.Unregister(element);
        }
    }
}