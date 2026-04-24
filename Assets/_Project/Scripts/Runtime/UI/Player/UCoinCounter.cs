using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player;
using StickmanIo.Runtime.Units;
using UnityEngine;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.UI
{
    public class UCoinCounter : UDynamicFloatField
    {
        GoldStorage goldStorage;

        protected override void OnStart()
        {
            goldStorage = GoldStorage.GetInstance;
            base.OnStart();
        }

        protected override float GetTargetValue()
        {
            return goldStorage.GetValue();
        }
    }
}