using System;

namespace StickmanIo.Runtime.Player
{
    public interface ILevel
    {
        int Level { get; }

        void AddLevel();

        event Action<int> OnLevelUp;
    }

    public class PlayerLevel : PlayerRigComponent, ILevel
    {
        int level = 1;

        public int Level => level;

        public event Action<int> OnLevelUp;

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
            OnLevelUp?.Invoke(level);
        }
    }
}