using GameDevUtils.Runtime;
using SaveableExtension.Runtime;
using StickmanProject.Runtime.SavePrefs;

namespace StickmanIo.Runtime.LevelDesign
{
    public class GoldStorage : AbstractResourceManager<GoldStorage>, ISaveableBehaviour<ProjectSavePrefs>
    {
        protected override float MinValue => 0;
        protected override float MaxValue => 999999f;

        protected override void OnAwake()
        {
            base.OnAwake();
            SaveableSupervisor.AddBehaviour(this);

            onChanged += SavePrefs;
        }

        protected override void OnDestroy() 
        {
            SaveableSupervisor.RemoveBehaviour(this);
            base.OnDestroy();
        }

        void SavePrefs()
        {
            SaveablePrefs.Save<ProjectSavePrefs>();
        }

        public void Serialize(ref ProjectSavePrefs savePrefs)
        {
            savePrefs.Coin = GetValueInt();
        }

        public void Deserialize(ProjectSavePrefs savePrefs)
        {
            SetValue(savePrefs.Coin);
        }
    }
}