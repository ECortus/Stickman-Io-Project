using System;
using GameDevUtils.Runtime;
using PurrNet;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEditor.SearchService;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface ILevel : IRigInterface
    {
        int Level { get; }

        void AddLevel();

        event Action<int> OnLevelUp;
    }

    public class PlayerLevel : PlayerRigComponent, ILevel
    {
        [SerializeField] SyncVar<int> level = new SyncVar<int>(0, ownerAuth: true);

        [SerializeField] int maximumKills;

        public int Level => level;

        public event Action<int> OnLevelUp;

        IPlayerSaveable playerSaveable;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!Rig.IsOwner)
            {
                return;
            }

            playerSaveable = Rig.Saveable;
            playerSaveable.OnSerialize += Serialize;
            playerSaveable.OnDeserialize += Deserialize;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        public void AddLevel()
        {
            level.value++;

            if (Level > maximumKills)
            {
                playerSaveable.TrySavePrefs();
            }

            OnLevelUp?.Invoke(Level);
        }

        void Serialize(ref ProjectSavePrefs savePrefs)
        {
            if (Level > maximumKills)
            {
                savePrefs.MaximumKills = Level;
                maximumKills = Level;
            }
        }

        void Deserialize(ProjectSavePrefs savePrefs)
        {
            maximumKills = savePrefs.MaximumKills;
        }
    }
}