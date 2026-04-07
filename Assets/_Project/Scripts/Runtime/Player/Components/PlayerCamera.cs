using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player.Data;
using Unity.Cinemachine;
using UnityEngine;
using CameraTarget = StickmanIo.Runtime.Camera.CameraTarget;

namespace StickmanIo.Runtime.Player
{
    public interface ICamera
    {
        public float RotationHorizontalAngle { get; }   
    }
    
    public class PlayerCamera : RigComponent, ICamera
    {
        CameraTarget cameraTarget;
        Vector2 lookDirection;
        
        PlayerData data;
        
        public float RotationHorizontalAngle => cameraTarget.GetRotation().eulerAngles.y;
        
        protected override void OnInitialize()
        {
            data = Data;
            
            var targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
            
            cameraTarget = ObjectInstantiator.InstantiateComponentOnNewGameObject<CameraTarget>($"{gameObject.name} Camera Target");
            cameraTarget.SetRotation(Data.CameraRotationOffset);
            
            targetGroup.Targets.Clear();
            targetGroup.AddMember(cameraTarget.Target, 1f, 1f);
            
            var inputEvents = Rig.InputEvents;
            inputEvents.OnLookAction += UpdateLookDirection;
        }
        
        void UpdateLookDirection(Vector2 dir)
        {
            lookDirection = dir;
        }
        
        private void Update()
        {
            var delta = Time.deltaTime;
            
            UpdateCameraTargetPosition();
            UpdateCameraRotation(delta);
        }

        void UpdateCameraTargetPosition()
        {
            var pos = Rig.transform.position + Data.CameraPositionOffset;
            SetCameraTargetPosition(pos);
        }

        void SetCameraTargetPosition(Vector3 pos)
        {
            cameraTarget.SetPosition(pos);
        }
        
        void UpdateCameraRotation(float delta)
        {
            if (lookDirection != Vector2.zero)
            {
                var anglesDelta = new Vector3(-lookDirection.y * data.LookVerticalSensitivity, 
                    lookDirection.x * data.LookHorizontalSensitivity, 0) * delta;
                
                var angles = cameraTarget.GetRotationAngles();
                angles = NormalizeEulesAngles(angles);
                
                angles += anglesDelta;

                var minX = data.CameraVerticalAnglesRange.x + data.CameraRotationOffset.x;
                var maxX = data.CameraVerticalAnglesRange.y + data.CameraRotationOffset.x;

                angles.x = Mathf.Clamp(angles.x, minX, maxX);
                
                cameraTarget.SetRotation(angles);
            }
        }

        Vector3 NormalizeEulesAngles(Vector3 angles)
        {
            angles.x = angles.x > 180f ? angles.x - 360f : angles.x;
            angles.y = angles.y > 180f ? angles.y - 360f : angles.y;
            angles.z = angles.z > 180f ? angles.z - 360f : angles.z;
            
            return angles;
        }
    }
}