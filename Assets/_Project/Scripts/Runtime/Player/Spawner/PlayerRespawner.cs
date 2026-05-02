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
            playerSpawner.SpawnPlayer(id.Value);
        }

        public void RespawnPlayer(PlayerID? playerID)
        {
            playerSpawner.SpawnPlayer(playerID.Value);
        }
    }
}