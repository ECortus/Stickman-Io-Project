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
        public bool IsOwned { get; private set; }
        public bool IsEquipped { get; private set; }

        public SkinData SkinData { get; private set; }

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

        List<SkinDataRuntime> skinsRuntimeData = new List<SkinDataRuntime>();

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
    }
}