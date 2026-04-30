using GameDevUtils.Runtime;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using Unity.Cinemachine;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface ICamera : IRigInterface
    {
        public float RotationHorizontalAngle { get; }   
    }
    
    public class PlayerCamera : PlayerRigComponent, ICamera
    {
        [SerializeField] CinemachineCamera cinemachineCamera;

        [SerializeField] CameraTarget cameraTarget;
        [SerializeField] Vector2 lookDirection;
        
        GlobalPlayerSettings settings;

        UnitView view;
        
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

            settings = Data.Settings;
            view = Rig.View;
            
            cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
            var targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
            
            cameraTarget = ObjectInstantiator.InstantiateComponentOnNewGameObject<CameraTarget>($"{gameObject.name} Camera Target");
            cameraTarget.SetRotation(settings.CameraRotationOffset);
            
            targetGroup.Targets.Clear();
            targetGroup.AddMember(cameraTarget.Target, 1f, 1f);

            ForceMoveCamera();
            
            var inputEvents = Rig.InputEvents;
            inputEvents.OnLookAction += UpdateLookDirection;
        }
        
        protected override void OnDestroyed()
        {
            if (!Rig.IsOwner)
            {
                return;
            }

            var parent = view.transform;
            cameraTarget.SetParent(parent);
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

        void ForceMoveCamera()
        {
            UpdateCameraTargetPosition();
            UpdateCameraRotation(999f);

            var pos = cameraTarget.transform.position;
            var rot = cameraTarget.transform.rotation;

            cinemachineCamera.ForceCameraPosition(pos, rot);
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