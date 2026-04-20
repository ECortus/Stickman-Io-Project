using System;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        [SerializeField] private int[] upgradesValues;

        public int GetValue(int lvl)
        {
            if (upgradesValues.Length == 0)
            {
                return 0;
            }

            if (lvl < 0)
            {
                return upgradesValues[0];
            }

            if (lvl >= upgradesValues.Length)
            {
                return upgradesValues[^1];
            }

            return upgradesValues[lvl];
        }

        public abstract void ApplyEffect(UnitRig unit, int lvl);
    }
}