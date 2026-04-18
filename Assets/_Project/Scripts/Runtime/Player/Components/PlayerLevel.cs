namespace StickmanIo.Runtime.Player
{
    public interface ILevel
    {
        int Level { get; }

        void AddLevel();
    }

    public class PlayerLevel : RigComponent, ILevel
    {
        int level = 1;

        public int Level => level;

        protected override void OnInitialize()
        {
            
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