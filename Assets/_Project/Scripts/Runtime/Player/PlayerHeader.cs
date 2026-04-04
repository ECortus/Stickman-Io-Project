using System;
using StickmanIo.Runtime.Player.Data;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerHeader : MonoBehaviour
    {
        [SerializeField] private PlayerData data;
        
        public PlayerData Data => data;
        
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitRig();
        }
        
        void InitRig()
        {
            var rig = gameObject.AddComponent<PlayerRig>();
            rig.Initialize(this);
        }
    }
}