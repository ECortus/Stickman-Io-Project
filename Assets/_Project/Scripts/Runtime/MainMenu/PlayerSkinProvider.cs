using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
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

    public class PlayerSkinProvider : SingletonMonoBehaviour<PlayerSkinProvider>, ISaveableBehaviour<ProjectSavePrefs>
    {
        [SerializeField] private SkinsCollection skinsCollection;
        [SerializeField] private List<string> defaultUnlockedSkinsIDs = new List<string> { "default" };

        List<SkinDataRuntime> skinsRuntimeData = new List<SkinDataRuntime>();

        string EquippedSkinID = "";
        SkinDataRuntime EquippedSkin = null;
        List<string> unlockedSkinsIDs = new List<string>();

        [SerializeField] private int[] colorRGB;
        [SerializeField] private Color currentColor;

        public event Action OnSkinEquipped;
        public event Action<Color> OnColorDeserialized;

        bool initialized = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            Initialize();
        }

        void Initialize()
        {
            if (initialized)
            {
                return;
            }

            AsyncTaskHelper.CreateTask(AsyncInitialize);

            initialized = true;
        }

        async UniTask AsyncInitialize()
        {
            await UniTask.WaitUntil(() => SaveableSupervisor.Exist());

            SaveableSupervisor.AddBehaviour(this);

            JoinUnlockedSkinsIDsAndDefaultUnlocked();
            CreateRuntimeDataList();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SaveableSupervisor.RemoveBehaviour(this);
        }

        void JoinUnlockedSkinsIDsAndDefaultUnlocked()
        {
            foreach (var id in defaultUnlockedSkinsIDs)
            {
                if (!unlockedSkinsIDs.Contains(id))
                {
                    unlockedSkinsIDs.Add(id);
                }
            }
        }

        void CreateRuntimeDataList()
        {
            skinsRuntimeData.Clear();

            var skins = skinsCollection.GetSkins();
            foreach (var skinData in skins)
            {
                var runtimeData = new SkinDataRuntime(skinData);
                if (unlockedSkinsIDs.Contains(skinData.Id))
                {
                    runtimeData.SetIsOwned();
                }

                skinsRuntimeData.Add(runtimeData);
            }

            SkinDataRuntime equippedSkin = null;
            if (EquippedSkinID != string.Empty)
            {
                equippedSkin = skinsRuntimeData.FirstOrDefault(s => s.SkinData.Id == EquippedSkinID);
            }
            else
            {
                equippedSkin = skinsRuntimeData.First();
            }
            SetEquippedSkin(equippedSkin, true, false);
        }

        public void SetEquippedSkin(SkinDataRuntime runtimeData, bool value, bool withSaving = true)
        {
            runtimeData.SetIsEquipped(value);

            if (value)
            {
                EquippedSkinID = runtimeData.SkinData.Id;
                EquippedSkin = runtimeData;

                if (withSaving)
                {
                    SavePrefs();
                }
            }

            OnSkinEquipped?.Invoke();
        }

        public void SetOwnedSkin(SkinDataRuntime runtimeData, bool withSaving = true)
        {
            runtimeData.SetIsOwned();
            unlockedSkinsIDs.Add(runtimeData.SkinData.Id);

            SetEquippedSkin(runtimeData, true, false);

            if (withSaving)
            {
                SavePrefs();
            }
        }

        public SkinDataRuntime GetEquippedSkinData()
        {
            if (!initialized)
            {
                Initialize();
            }

            return skinsRuntimeData.FirstOrDefault(s => s.IsEquipped);
        }

        public List<SkinDataRuntime> GetSkinsRuntimeData()
        {
            if (!initialized)
            {
                Initialize();
            }

            return skinsRuntimeData;
        }

        public Color GetCurrentColor()
        {
            return currentColor;
        }

        void SavePrefs()
        {
            SaveablePrefs.Save<ProjectSavePrefs>();
        }

        public void Serialize(ref ProjectSavePrefs savePrefs)
        {
            savePrefs.EquippedSkinID = EquippedSkinID;
            savePrefs.UnlockedSkinIDs = unlockedSkinsIDs.ToArray();

            savePrefs.ColorRGB = ProjectSavePrefs.ColorToArray(currentColor);
        }

        public void Deserialize(ProjectSavePrefs savePrefs)
        {
            EquippedSkinID = savePrefs.EquippedSkinID;
            unlockedSkinsIDs = savePrefs.UnlockedSkinIDs.ToList();

            var rgb = savePrefs.ColorRGB;
            var color = ProjectSavePrefs.ArrayToColor(rgb);

            colorRGB = rgb;
            currentColor = color;

            OnColorDeserialized?.Invoke(color);
        }

        public void OnColorChanged(Color color)
        {
            colorRGB = ProjectSavePrefs.ColorToArray(color);
            currentColor = color;

            SavePrefs();
        }
    }
}