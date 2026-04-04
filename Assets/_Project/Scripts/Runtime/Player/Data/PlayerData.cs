using UnityEngine;

namespace StickmanIo.Runtime.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StickmanIo/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public float Speed = 5f;
    }
}