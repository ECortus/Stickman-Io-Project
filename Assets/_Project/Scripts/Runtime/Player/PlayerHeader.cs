using System;
using GameDevUtils.Runtime;
using PurrNet;
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

        protected override void OnSpawned()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitRig();
        }

        void InitRig()
        {
            rig = gameObject.AddComponent<PlayerRig>();

            var playerID = localPlayer;
            rig.SetIsSpawned(true, false);
            rig.GiveOwnership(playerID);

            rig.Initialize(this);

            OnRigInitialize?.Invoke();
        }

        public readonly FireEvent OnRigInitialize = new FireEvent();
    }
}