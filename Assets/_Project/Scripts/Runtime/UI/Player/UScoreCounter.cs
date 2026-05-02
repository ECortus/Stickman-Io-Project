using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.Units;
using UnityEngine;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.UI
{
    public class UScoreCounter : UDynamicFloatField
    {
        UnitsManager unitsManager;

        PlayerRig rig;

        protected override void OnStart()
        {
            unitsManager = UnitsManager.GetInstance;
            unitsManager.OnOwnerRigChanged += SetPlayerRig;

            base.OnStart();
        }

        void SetPlayerRig()
        {
            rig = unitsManager.OwnerRig;
        }

        protected override float GetTargetValue()
        {
            if (rig == null)
            {
                return 0;
            }

            var resources = rig.Resources;
            return resources.Score;
        }
    }
}