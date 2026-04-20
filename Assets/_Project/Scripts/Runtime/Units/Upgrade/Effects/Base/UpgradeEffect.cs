using System;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface IGradeable
    {
        
    }

    public abstract class UpgradeEffect : ScriptableObject
    {
        [SerializeField] private float[] upgradesValues;

        public float GetFullValue(int lvl)
        {
            float value = 0f;
            for (int i = 0; i < lvl; i++)
            {
                value += GetValue(i);
            }

            return value;
        }

        float GetValue(int lvl)
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

        public abstract void ApplyEffect(UnitRig unit, int lvl, float valueModifier = 1f);
    }
}