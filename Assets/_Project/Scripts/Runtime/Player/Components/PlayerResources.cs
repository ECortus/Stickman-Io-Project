using System.Diagnostics;
using GameDevUtils.Runtime;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IResources : IRigInterface
    {
        int Score { get; }

        int CalculateScore(IPlayerRig killedRig);

        int CalculateScore(int lvl);

        void AddScore(int add);

        void AddCoins(int coins);
    }

    public class PlayerResources : PlayerRigComponent, IResources
    {
        [SerializeField] int score;
        [SerializeField] int maximumScore;

        public int Score => score;
        GoldStorage goldStorage;

        IPlayerSaveable playerSaveable;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!Rig.IsOwner)
            {
                return;
            }

            goldStorage = GoldStorage.GetInstance;

            playerSaveable = Rig.Saveable;
            playerSaveable.OnSerialize += Serialize;
            playerSaveable.OnDeserialize += Deserialize;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        public int CalculateScore(IPlayerRig killedRig)
        {
            return CalculateScore(killedRig.Level.Level);
        }

        public int CalculateScore(int lvl)
        {
            var settings = Data.Settings;

            var baseScore = settings.BaseScorePerKill;
            var multiplierPerLevel = settings.ScoreMultiplierPerEachLevelPerKill;

            var calculated = baseScore + multiplierPerLevel * lvl;
            return calculated;
        }

        public void AddScore(int add)
        {
            score += add;
            if (score > maximumScore)
            {
                playerSaveable.TrySavePrefs();
            }
        }

        public void AddCoins(int amount)
        {
            if (!Rig.IsOwner)
            {
                return;
            }

            if (amount <= 0)
            {
                return;
            }

            goldStorage.Add(amount);
        }

        void Serialize(ref ProjectSavePrefs savePrefs)
        {
            if (score > maximumScore)
            {
                savePrefs.MaximumScore = score;
                maximumScore = score;
            }
        }

        void Deserialize(ProjectSavePrefs savePrefs)
        {
            maximumScore = savePrefs.MaximumScore;
        }
    }
}