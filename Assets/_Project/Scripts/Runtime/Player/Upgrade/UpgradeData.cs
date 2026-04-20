using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    [CreateAssetMenu(fileName = "Upgrade00Data", menuName = "StickmanIo/Upgrades/Data", order = 0)]
    public class UpgradeData : ScriptableObject
    {
        public string id;

        [Space(5)]
        public string title;
        public Sprite icon;

        //TODO: Action of upgrade, via scriptableobject or choosing script
    }
}