using StickmanIo.Runtime.LevelDesign;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerLoot : PlayerRigComponent
    {
        IHealth health;
        LootManager lootManager;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            health = GetComponentInParent<IHealth>();
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

            var minCoinCost = settings.MinCoinCost;
            var maxCoinCost = settings.MaxCoinCost;

            var minCoins = settings.MinCoinsCount;
            var maxCoins = settings.MaxCoinsCount;

            var offset = new Vector3(0, 1f, 0);

            int count = Random.Range(minCoins, maxCoins + 1);
            int coinCost = Random.Range(minCoinCost, maxCoinCost + 1);

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