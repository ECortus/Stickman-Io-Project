using StickmanIo.Runtime.Units;

namespace StickmanIo.Runtime.Player
{
    public interface IResources : IRigInterface
    {
        int Coins { get; }

        void AddCoins(int coins);
    }

    public class PlayerResources : PlayerRigComponent, IResources
    {
        int coins;

        public int Coins => coins;

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