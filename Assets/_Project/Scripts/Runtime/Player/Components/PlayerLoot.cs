using StickmanIo.Runtime.LevelDesign;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerLoot : PlayerRigComponent
    {
        IHealth health;
        ILevel level;

        LootManager lootManager;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            health = Rig.Health;
            level = Rig.Level;

            lootManager = LootManager.GetInstance;

            health.OnDied += SpawnLoot;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        void SpawnLoot()
        {
            var settings = Data.Settings;

            var minCoins = settings.MinCoinsCount;
            var maxCoins = settings.MaxCoinsCount;

            var offset = new Vector3(0, 1f, 0);

            int count = Random.Range(minCoins, maxCoins + 1);
            int coinCost = Mathf.Max(level.Level + 1, 1);

            float throwForce = settings.throwForceOfCoin;

            var destination = transform.position + offset;
            var parent = transform.parent;

            for (int i = 0; i < count; i++)
            {
                var instance = lootManager.InstantiateCoinLoot(coinCost, destination, parent);

                instance.SetExcludedEntities(GetEntityId());

                var randomDirection = Random.insideUnitSphere;
                randomDirection = randomDirection.normalized;
                randomDirection.y = Mathf.Abs(randomDirection.y);

                instance.Throw(randomDirection, throwForce);
            }
        }
    }
}