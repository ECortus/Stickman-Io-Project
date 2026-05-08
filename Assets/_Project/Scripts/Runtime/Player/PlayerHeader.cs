using System;
using GameDevUtils.Runtime;
using PurrNet;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player.Data;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerHeader : NetworkIdentity
    {
        [SerializeField] private PlayerData data;

        PlayerRig rig;

        public bool IsOwner => isOwner;
        public PlayerData Data => data;

        public PlayerRig Rig => rig;

        void Awake()
        {
            InitRig();
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();

            GameStatement gameStatement = GameStatement.GetInstance;
            if (gameStatement.IsNone || gameStatement.IsLoading)
            {
                gameStatement.SetPlay();
            }

            InitComponents();
        }

        void InitRig()
        {
            rig = gameObject.AddComponent<PlayerRig>();

            rig.Initialize(this);

            OnRigInitialize?.Invoke();
        }

        void InitComponents()
        {
            rig.OnInitializeComponentsComplete();
        }

        public readonly FireEvent OnRigInitialize = new FireEvent();
    }
}