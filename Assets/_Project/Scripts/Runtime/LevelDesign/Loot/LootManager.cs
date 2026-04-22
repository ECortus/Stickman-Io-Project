using GameDevUtils.Runtime;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public class LootManager : SingletonMonoBehaviour<LootManager>
    {
        [SerializeField] private PickupAbleObject coinPickupAblePrefab = null;

        public PickupAbleObject InstantiateCoinLoot(int amount, Vector3 position, Transform parent = null)
        {
            var instance = InstantiateLoot(coinPickupAblePrefab, amount, position, parent);
            return instance;
        }

        PickupAbleObject InstantiateLoot(PickupAbleObject prefab, int amount, Vector3 position, Transform parent = null)
        {
            return InstantiateLoot(prefab, amount, position, Quaternion.identity, parent);
        }

        PickupAbleObject InstantiateLoot(PickupAbleObject prefab, int amount, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = ObjectInstantiator.InstantiatePrefabForComponent(prefab, position, rotation, parent);
            instance.SetAmount(amount);

            return instance;
        }
    }
}