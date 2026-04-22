using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public class CoinPickupAble : PickupAbleObject
    {
        protected override void AddResource(PlayerRig rig, int amount)
        {
            var resources = rig.Resources;
            resources.AddCoins(amount);
        }
    }
}
