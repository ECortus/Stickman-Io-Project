using System;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Input;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public class GameStatement : SingletonMonoBehaviour<GameStatement>
    {
        [Serializable]
        public enum EState
        {
            None,
            Loading,
            Playing,
            Paused,
            Dead
        }

        [SerializeField, ReadOnly] EState state = EState.None;

        public bool IsLoading => state == EState.Loading;
        public bool IsPlaying => state == EState.Playing;
        public bool IsPaused => state == EState.Paused;
        public bool IsDead => state == EState.Dead;

        void SetState(EState newState)
        {
            if (state == newState)
                return;

            switch (newState)
            {
                case EState.Loading:
                    OnLoading();
                    break;
                case EState.Playing:
                    OnPlaying();
                    break;
                case EState.Paused:
                    OnPaused();
                    break;
                case EState.Dead:
                    OnDead();
                    break;
                default:
                    break;
            }

            state = newState;
        }

        public void SetLoading() => SetState(EState.Loading);
        public void SetPlay() => SetState(EState.Playing);
        public void SetPause() => SetState(EState.Paused);
        public void SetDead() => SetState(EState.Dead);

        void OnLoading()
        {
            CursorViewController.Enable();
        }

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