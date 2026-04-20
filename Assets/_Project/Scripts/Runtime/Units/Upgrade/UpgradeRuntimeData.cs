using System;

namespace StickmanIo.Runtime.Units
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

            var effect = data.effect;
            effect.ApplyEffect(owner, lvl);
        }
    }
}