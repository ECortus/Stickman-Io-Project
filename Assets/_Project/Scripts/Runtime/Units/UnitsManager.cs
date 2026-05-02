using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public class UnitsManager : AbstractComponentManager<UnitRig, UnitsManager>
    {
        [SerializeField] private PlayerRig ownerRig;

        public PlayerRig OwnerRig => ownerRig;
        public event Action OnOwnerRigChanged;

        public override void Register(UnitRig element)
        {
            if (element is PlayerRig playerRig)
            {
                if (playerRig.IsOwner)
                {
                    if (OwnerRig && OwnerRig != playerRig)
                    {
                        Debug.LogWarning($"OwnerRig already set as {OwnerRig.name}, it can be only ONE owner on scene. Another os {element.name}");
                        /* throw new System.Exception("OwnerRig already set, it can be only ONE owner on scene."); */
                    }

                    ownerRig = playerRig;
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

                    ownerRig = null;
                    OnOwnerRigChanged?.Invoke();
                }
            }

            base.Unregister(element);
        }
    }
}