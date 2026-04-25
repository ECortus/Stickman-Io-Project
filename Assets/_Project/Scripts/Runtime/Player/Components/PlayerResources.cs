using SaveableExtension.Runtime;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;

namespace StickmanIo.Runtime.Player
{
    public interface IResources : IRigInterface
    {
        int Score { get; }
        void AddScore(IPlayerRig killedRig);

        int Coins { get; }
        void AddCoins(int coins);
    }

    public class PlayerResources : PlayerRigComponent, IResources, ISaveableBehaviour<ProjectSavePrefs>
    {
        int score;
        int coins;

        public int Score => score;
        public int Coins => coins;

        bool isOwner;
        GoldStorage goldStorage;

        ProjectSavePrefs prefs;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            isOwner = Rig.IsOwner;

            if (!isOwner)
            {
                enabled = false;
                return;
            }

            goldStorage = GoldStorage.GetInstance;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        public void AddScore(IPlayerRig killedRig)
        {
            var settings = Data.Settings;

            var baseScore = settings.BaseScorePerKill;
            var multiplierPerLevel = settings.ScoreMultiplierPerEachLevelPerKill;

            var score = baseScore + multiplierPerLevel * killedRig.Level.Level;
            this.score += score;

            if (score > prefs.MaximumScore)
            {
                SavePrefs();
            }
        }

        public void AddCoins(int amount)
        {
            if (!isOwner)
            {
                return;
            }

            if (amount <= 0)
            {
                return;
            }

            goldStorage.Add(amount);
            this.coins += amount;
        }

        void SavePrefs()
        {
            SaveablePrefs.Save<ProjectSavePrefs>();
        }

        public void Serialize(ref ProjectSavePrefs savePrefs)
        {
            savePrefs.MaximumScore = score;
        }

        public void Deserialize(ProjectSavePrefs savePrefs)
        {
            prefs = savePrefs;
        }
    }
}