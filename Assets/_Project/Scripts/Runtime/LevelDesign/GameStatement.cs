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
            Dead
        }

        State state = State.None;

        public bool IsPlaying => state == State.Playing;
        public bool IsPaused => state == State.Paused;
        public bool IsDead => state == State.Dead;

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
                case State.Dead:
                    OnDead();
                    break;
                default:
                    break;
            }

            state = newState;
        }

        public void SetPlay() => SetState(State.Playing);
        public void SetPause() => SetState(State.Paused);
        public void SetDead() => SetState(State.Dead);

        void OnPlaying()
        {
            CursorViewController.Disable();
        }

        void OnPaused()
        {
            CursorViewController.Enable();
        }

        void OnDead()
        {
            CursorViewController.Enable();
        }
    }
}