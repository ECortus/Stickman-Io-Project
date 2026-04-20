using System;

namespace StickmanIo.Runtime.Units
{
    [Serializable]
    public class UpgradeRuntimeData
    {
        public UpgradeRuntimeData(UpgradeData dt, UnitRig rig)
        {
            data = dt;
            owner = rig;

            lvl = 0;
        }

        public UpgradeRuntimeData(UpgradeData dt, UnitRig rig, int startLvl)
        {
            data = dt;
            owner = rig;

            lvl = startLvl;
        }

        UnitRig owner;

        UpgradeData data;
        int lvl;

        public UpgradeData Data => data;
        public int Level => lvl;

        public event Action OnLevelUp;

        public void Upgrade()
        {
            lvl++;
            OnLevelUp?.Invoke();

            data.ApplyEffect(owner, lvl);
        }
    }
}