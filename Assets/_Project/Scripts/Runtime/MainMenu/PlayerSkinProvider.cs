using System;
using System.Collections.Generic;
using System.Linq;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.MainMenu
{
    [Serializable]
    public class SkinDataRuntime
    {
        [field: SerializeField] public bool IsOwned { get; private set; }
        [field: SerializeField] public bool IsEquipped { get; private set; }

        [field: SerializeField] public SkinData SkinData { get; private set; }

        public SkinDataRuntime(SkinData skinData)
        {
            SkinData = skinData;
            IsOwned = false;
            IsEquipped = false;
        }

        public void SetIsOwned()
        {
            IsOwned = true;
        }

        public void SetIsEquipped(bool equipped)
        {
            IsEquipped = equipped;
        }
    }

    public class PlayerSkinProvider : SingletonMonoBehaviour<PlayerSkinProvider>
    {
        [SerializeField] private SkinsCollection skinsCollection;
        [SerializeField] private List<string> defaultUnlockedSkinsIDs = new List<string> { "default" };

        [SerializeField] private List<SkinDataRuntime> skinsRuntimeData = new List<SkinDataRuntime>();

        public event Action OnSkinEquipped;

        public SkinDataRuntime GetEquippedSkinData()
        {
            if (skinsRuntimeData.Count == 0)
            {
                CreateRuntimeDataList();
            }   

            return skinsRuntimeData.FirstOrDefault(s => s.IsEquipped);
        }

        public List<SkinDataRuntime> GetSkinsRuntimeData()
        {
            if (skinsRuntimeData.Count == 0)
            {
                CreateRuntimeDataList();
            }

            return skinsRuntimeData;
        }

        void CreateRuntimeDataList()
        {
            skinsRuntimeData.Clear();

            var skins = skinsCollection.GetSkins();
            foreach (var skinData in skins)
            {
                var runtimeData = new SkinDataRuntime(skinData);

                // Set default unlocked skins as owned and equipped IF none other in save
                if (defaultUnlockedSkinsIDs.Contains(skinData.Id))
                {
                    runtimeData.SetIsOwned();
                    runtimeData.SetIsEquipped(true);
                }

                skinsRuntimeData.Add(runtimeData);
            }
        }

        public void SetEquippedSkin(SkinDataRuntime runtimeData, bool value)
        {
            runtimeData.SetIsEquipped(value);
            OnSkinEquipped?.Invoke();
        }

        public void SetOwnedSkin(SkinDataRuntime runtimeData)
        {
            runtimeData.SetIsOwned();
        }
    }
}