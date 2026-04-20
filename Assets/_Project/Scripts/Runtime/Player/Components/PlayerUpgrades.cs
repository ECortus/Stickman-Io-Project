using System.Collections.Generic;

namespace StickmanIo.Runtime.Player
{
    public interface IUpgrades
    {
        int AvailableUpgrades { get; }
        bool HasAvailableUpgrade();
        void ReduceAvailableUpgrades();

        bool UpgradesInitialized { get; }
        List<UpgradeRuntimeData> RuntimeUpgrades { get; }
    }

    public class PlayerUpgrades : PlayerRigComponent, IUpgrades
    {
        int availableUpgrades;
        List<UpgradeRuntimeData> upgrades = new List<UpgradeRuntimeData>();

        ILevel level;

        public int AvailableUpgrades => availableUpgrades;
        public bool HasAvailableUpgrade() => availableUpgrades > 0;

        public bool UpgradesInitialized => upgrades.Count == Data.Upgrades.Length;
        public List<UpgradeRuntimeData> RuntimeUpgrades => upgrades;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            level = Rig.Level;
            level.OnLevelUp += OnLevelUp;

            InitializeUpgrades();
        }

        void InitializeUpgrades()
        {
            var upgradesData = Data.Upgrades;
            for (int i = 0; i < upgradesData.Length; i++)
            {
                var data = upgradesData[i];
                if (data != null)
                {
                    var newUpgrade = new UpgradeRuntimeData(data);
                    upgrades.Add(newUpgrade);
                }
            }
        }

        void OnLevelUp(int lvl)
        {
            AddAvailableUpgrades();
        }

        void AddAvailableUpgrades()
        {
            availableUpgrades++;
        }

        public void ReduceAvailableUpgrades()
        {
            availableUpgrades--;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }
    }
}