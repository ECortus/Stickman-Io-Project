using System;
using UnityEngine;

namespace StickmanIo.Runtime.Player.Data
{
    [CreateAssetMenu(fileName = "GlobalPlayerSettings", menuName = "StickmanIo/Data/GlobalPlayerSettings", order = 0)]
    public class GlobalPlayerSettings : ScriptableObject
    {
        [Header("Rigidbody parameters")]
        public float Mass = 50f;

        [Header("Movement parameters")]
        public float Speed = 5f;
        public float SpeedInAir = 3f;

        [Space(5)]
        public float RotationSpeed = 270f;

        [Space(5)]
        public float JumpForce = 5f;

        [Header("Camera parameters")]
        public Vector3 CameraPositionOffset = new Vector3(0f, 1.4f, 0f);
        public Vector3 CameraRotationOffset = new Vector3(25f, 0f, 0f);
        public Vector2 CameraVerticalAnglesRange = new Vector2(15f, 15f);

        [Space(5)]
        public float LookVerticalSensitivity = 5f;
        public float LookHorizontalSensitivity = 25f;

        [Header("Health parameters")]
        public float BaseMaxHealth = 100f;

        [Header("Attack parameters")]
        public float BaseAttackDamage = 0.5f;
        public float AttackInputsDelay = 0.4f;
        public AttackData[] AttacksData;

        [Serializable]
        public struct ShakeData
        {
            public float duration;
            public float force;
            public float decreaseFactor;
            public AnimationCurve curve;
        }

        [Space(5)]
        public ShakeData cameraShakeOnAttack;
        public ShakeData cameraShakeOnHit;

        [System.Serializable]
        public struct AttackData
        {
            public float PrepareDuration;
            public float AttackDuration;
            public float PostDuration;

            [Space(5), Range(1f, 3f)]
            public float DamageModificator;
        }

        [Header("Loot parameters")]
        public int BaseScorePerKill = 100;
        public int ScoreMultiplierPerEachLevelPerKill = 200;

        [Space(10)]
        public int MinCoinsCount = 10;
        public int MaxCoinsCount = 20;

        [Space(5)]
        public float throwForceOfCoin = 60f;

        [Header("Animations parameters")]
        public float blickDurationOnHit = 0.5f;
        public float blickFrequencyOnHit = 1f;
    }
}