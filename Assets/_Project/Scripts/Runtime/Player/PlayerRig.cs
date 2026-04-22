using System;
using System.Collections.Generic;
using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IPlayerRig
    {
        bool IsOwner { get; }
        
        IHealth Health { get; }
        IAttacker Attacker { get; }
        ILevel Level { get; }
        
        ICamera Camera { get; }
        IMovement Movement { get; }
        
        IPlayerGroundCheck GroundCheck { get; }
        
        IUpgrades Upgrades { get; }
        IResources Resources { get; }
        IInputEvents InputEvents { get; }
    }

    public class PlayerRig : UnitRig, IPlayerRig
    {
        PlayerHeader header;

        PlayerHealth health;
        PlayerAttacker attacker;
        PlayerLevel level;
        
        PlayerCamera cam;
        PlayerMovement movement;

        PlayerUpgrades upgrades;
        PlayerResources resources;
        PlayerLoot loot;

        PlayerInputEvents inputEvents;
        
        IPlayerGroundCheck groundCheck;

        public bool IsOwner => header.IsOwner;
        
        public IHealth Health => health;
        public IAttacker Attacker => attacker;
        public ILevel Level => level;
        
        public ICamera Camera => cam;
        public IMovement Movement => movement;
        
        public IPlayerGroundCheck GroundCheck => groundCheck;
        
        public IUpgrades Upgrades => upgrades;
        public IResources Resources => resources;
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
            
            upgrades = AddComponent<PlayerUpgrades>();
            resources = AddComponent<PlayerResources>();
            loot = AddComponent<PlayerLoot>();

            inputEvents = AddComponent<PlayerInputEvents>();

            groundCheck = GetComponentInChildren<IPlayerGroundCheck>();
        }
    }
}