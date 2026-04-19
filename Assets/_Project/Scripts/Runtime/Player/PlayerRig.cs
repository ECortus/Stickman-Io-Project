using System;
using System.Collections.Generic;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRig : UnitRig
    {
        PlayerHeader header;

        PlayerHealth health;
        PlayerAttacker attacker;
        PlayerLevel level;
        
        PlayerCamera cam;
        PlayerMovement movement;

        PlayerInputEvents inputEvents;
        
        IPlayerGroundCheck groundCheck;

        public bool IsOwner => header.IsOwner;
        
        public IHealth Health => health;
        public IAttacker Attacker => attacker;
        public ILevel Level => level;
        
        public ICamera Camera => cam;
        public IMovement Movement => movement;
        
        public IPlayerGroundCheck GroundCheck => groundCheck;
        
        public IInputEvents InputEvents => inputEvents;

        public PlayerData GetData() => header.Data;
        
        public void Initialize(PlayerHeader hdr)
        {
            header = hdr;
            Initialize();
        }
        
        protected override void InitializeComponents()
        {
            health = AddComponent<PlayerHealth>();
            attacker = AddComponent<PlayerAttacker>();
            level = AddComponent<PlayerLevel>();
            
            cam = AddComponent<PlayerCamera>();
            movement = AddComponent<PlayerMovement>();
            
            inputEvents = AddComponent<PlayerInputEvents>();

            groundCheck = GetComponentInChildren<IPlayerGroundCheck>();
        }
    }
}