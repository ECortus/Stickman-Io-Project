namespace StickmanIo.Runtime.Player
{
    public interface ILevel
    {
        int Level { get; }

        void AddLevel();
    }

    public class PlayerLevel : PlayerRigComponent, ILevel
    {
        int level = 1;

        public int Level => level;

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnDestroyed()
        {
            
        }

        public void AddLevel()
        {
            level++;
        }
    }
}