using System;
using GameDevUtils.Runtime;
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
        [SerializeField] int level = 0;
        bool isOwner;

        [SerializeField] int maximumKills;

        public int Level => level;

        public event Action<int> OnLevelUp;

        IPlayerSaveable playerSaveable;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            isOwner = Rig.IsOwner;

            if (!isOwner)
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
            level++;

            if (level > maximumKills)
            {
                playerSaveable.TrySavePrefs();
            }

            OnLevelUp?.Invoke(level);
        }

        void Serialize(ref ProjectSavePrefs savePrefs)
        {
            if (level > maximumKills)
            {
                savePrefs.MaximumKills = level;
                maximumKills = level;
            }
        }

        void Deserialize(ProjectSavePrefs savePrefs)
        {
            maximumKills = savePrefs.MaximumKills;
        }
    }
}