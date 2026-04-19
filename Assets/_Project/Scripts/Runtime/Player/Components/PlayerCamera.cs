using GameDevUtils.Runtime;
using StickmanIo.Runtime.Player.Data;
using Unity.Cinemachine;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface ICamera
    {
        public float RotationHorizontalAngle { get; }   
    }
    
    public class PlayerCamera : PlayerRigComponent, ICamera
    {
        CameraTarget cameraTarget;
        Vector2 lookDirection;
        
        GlobalPlayerSettings settings;
        
        public float RotationHorizontalAngle
        {
            get
            {
                if (cameraTarget != null)
                {
                    var angles = cameraTarget.GetRotationAngles();
                    return angles.y;
                }

                return 0f;
            }
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!Rig.IsOwner)
            {
                enabled = false;
                return;
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            settings = Data.Settings;
            
            var targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
            
            cameraTarget = ObjectInstantiator.InstantiateComponentOnNewGameObject<CameraTarget>($"{gameObject.name} Camera Target");
            cameraTarget.SetRotation(settings.CameraRotationOffset);
            
            targetGroup.Targets.Clear();
            targetGroup.AddMember(cameraTarget.Target, 1f, 1f);
            
            var inputEvents = Rig.InputEvents;
            inputEvents.OnLookAction += UpdateLookDirection;
        }
        
        protected override void OnDestroyed()
        {
            
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
            var pos = Rig.transform.position + settings.CameraPositionOffset;
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
                var anglesDelta = new Vector3(-lookDirection.y * settings.LookVerticalSensitivity, 
                    lookDirection.x * settings.LookHorizontalSensitivity, 0) * delta;
                
                var angles = cameraTarget.GetRotationAngles();
                angles = NormalizeEulesAngles(angles);
                
                angles += anglesDelta;

                var minX = settings.CameraVerticalAnglesRange.x + settings.CameraRotationOffset.x;
                var maxX = settings.CameraVerticalAnglesRange.y + settings.CameraRotationOffset.x;

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