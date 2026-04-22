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

        public int Score => score;
        public int Coins => coins;

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
            if (amount <= 0)
            {
                return;
            }

            this.coins += amount;
        }
    }
}