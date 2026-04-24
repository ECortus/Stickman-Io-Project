using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Units;

namespace StickmanIo.Runtime.Player
{
    public interface IResources : IRigInterface
    {
        int Score { get; }
        void AddScore(IPlayerRig killedRig);

        int Coins { get; }
        void AddCoins(int coins);
    }

    public class PlayerResources : PlayerRigComponent, IResources
    {
        int score;
        int coins;

        bool isOwner;
        GoldStorage goldStorage;

        public int Score => score;
        public int Coins => coins;

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
    }
}