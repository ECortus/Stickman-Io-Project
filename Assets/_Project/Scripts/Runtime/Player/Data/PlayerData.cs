using UnityEngine;
using StickmanIo.Runtime.Units;

namespace StickmanIo.Runtime.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StickmanIo/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public GlobalPlayerSettings Settings;
        public SkinsCollection SkinsCollection;

        [Space(5)]
        public UpgradeData[] Upgrades;
    }
}