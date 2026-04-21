using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.Units;
using UnityEngine;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCoinCounter : UDynamicFloatField
    {
        UnitsManager unitsManager;

        PlayerRig rig;

        protected override void OnStart()
        {
            unitsManager = UnitsManager.GetInstance;
            base.OnStart();
        }

        protected override void OnUpdate()
        {
            if (rig == null && unitsManager.OwnerRig != null)
            {
                rig = unitsManager.OwnerRig;
            }
        }

        protected override float GetTargetValue()
        {
            if (rig == null)
            {
                return 0;
            }

            var resources = rig.Resources;
            return resources.Coins;
        }
    }
}