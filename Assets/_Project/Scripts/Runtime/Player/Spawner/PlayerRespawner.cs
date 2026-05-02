using GameDevUtils.Runtime;
using PurrNet;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
    {
        [SerializeField] PlayerSpawner playerSpawner;

        public void RespawnPlayer(NetworkIdentity player)
        {
            var id = player.localPlayer;
            playerSpawner.RespawnPlayer(id.Value);
        }

        public void RespawnPlayer(PlayerID? playerID)
        {
            playerSpawner.RespawnPlayer(playerID.Value);
        }
    }
}