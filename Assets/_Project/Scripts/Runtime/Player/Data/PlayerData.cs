using UnityEngine;

namespace StickmanIo.Runtime.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StickmanIo/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Rigidbody parameters")]
        public float Mass = 50f;
        
        [Header("Movement parameters")]
        public float Speed = 5f;
        public float RotationSpeed = 270f;
        public float JumpForce = 5f;

        [Header("Camera parameters")] 
        public Vector3 CameraPositionOffset = new Vector3(0f, 1.4f, 0f);
        public Vector3 CameraRotationOffset = new Vector3(25f, 0f, 0f);
        public Vector2 CameraVerticalAnglesRange = new Vector2(15f, 15f);
        
        [Space(5)]
        public float LookVerticalSensitivity = 5f;
        public float LookHorizontalSensitivity = 25f;
    }
}