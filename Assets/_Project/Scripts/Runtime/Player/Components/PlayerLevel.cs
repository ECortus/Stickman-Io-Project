using System;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEditor.SearchService;

namespace StickmanIo.Runtime.Player
{
    public interface ILevel : IRigInterface
    {
        int Level { get; }

        void AddLevel();

        event Action<int> OnLevelUp;
    }

    public class PlayerLevel : PlayerRigComponent, ILevel, ISaveableBehaviour<ProjectSavePrefs>
    {
        int level = 0;

        public int Level => level;

        public event Action<int> OnLevelUp;

        ProjectSavePrefs prefs;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SaveableSupervisor.AddBehaviour(this);
        }

        protected override void OnDestroyed()
        {
            SaveableSupervisor.RemoveBehaviour(this);
        }

        public void AddLevel()
        {
            level++;
            OnLevelUp?.Invoke(level);

            if (level > prefs.MaximumKills)
            {
                SavePrefs();
            }
        }

        void SavePrefs()
        {
            SaveablePrefs.Save<ProjectSavePrefs>();
        }

        public void Serialize(ref ProjectSavePrefs savePrefs)
        {
            savePrefs.MaximumKills = level;
        }

        public void Deserialize(ProjectSavePrefs savePrefs)
        {
            prefs = savePrefs;
        }
    }
}