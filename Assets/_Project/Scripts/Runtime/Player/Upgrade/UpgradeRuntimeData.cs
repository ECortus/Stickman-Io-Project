using System;

namespace StickmanIo.Runtime.Player
{
    [Serializable]
    public class UpgradeRuntimeData
    {
        public UpgradeRuntimeData(UpgradeData dt)
        {
            data = dt;
            lvl = 0;
        }

        public UpgradeRuntimeData(UpgradeData dt, int startLvl)
        {
            data = dt;
            lvl = startLvl;
        }

        UpgradeData data;
        int lvl;

        public UpgradeData Data => data;
        public int Level => lvl;

        public event Action OnLevelUp;

        public void AddLevel()
        {
            lvl++;

            //TODO: Upgrade from data
            OnLevelUp?.Invoke();
        }
    }
}