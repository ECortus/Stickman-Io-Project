using GameDevUtils.Runtime;
using StickmanIo.Runtime.Input;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public class GameStatement : SingletonMonoBehaviour<GameStatement>
    {
        public enum State
        {
            None,
            Playing,
            Paused,
        }

        State state = State.None;

        public bool IsPlaying => state == State.Playing;
        public bool IsPaused => state == State.Paused;

        void Start()
        {
            SetPlay();
        }

        void SetState(State newState)
        {
            if (state == newState)
                return;

            switch (newState)
            {
                case State.Playing:
                    OnPlaying();
                    break;
                case State.Paused:
                    OnPaused();
                    break;
                default:
                    break;
            }

            state = newState;
        }

        public void SetPlay() => SetState(State.Playing);
        public void SetPause() => SetState(State.Paused);

        void OnPlaying()
        {
            CursorViewController.Disable();
        }

        void OnPaused()
        {
            CursorViewController.Enable();
        }
    }
}