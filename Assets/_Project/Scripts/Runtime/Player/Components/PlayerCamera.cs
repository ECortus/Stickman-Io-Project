using System;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;
using Unity.Cinemachine;
using UnityEngine;
using static StickmanIo.Runtime.Player.Data.GlobalPlayerSettings;

namespace StickmanIo.Runtime.Player
{
    public interface ICamera : IRigInterface
    {
        float RotationHorizontalAngle { get; }   

        void ShakeOnAttack();
        void ShakeOnHit();
    }
    
    public class PlayerCamera : PlayerRigComponent, ICamera
    {
        [SerializeField] CinemachineCamera cinemachineCamera;

        [SerializeField] CameraTarget cameraTarget;
        [SerializeField] Vector2 lookDirection;
        
        GlobalPlayerSettings settings;

        UnitView view;

        bool initialized = false;
        
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

            shakeOffset = Vector3.zero;

            initialized = true;
        }
        
        protected override void OnDestroyed()
        {
            if (cameraTarget)
            {
                ObjectHelper.Destroy(cameraTarget.gameObject);
            }
        }
        
        void UpdateLookDirection(Vector2 dir)
        {
            lookDirection = dir;
        }
        
        private void Update()
        {
            if (!initialized)
            {
                return;
            }

            var delta = Time.deltaTime;

            UpdateShakeCamera(delta);
            
            UpdateCameraTargetPosition();
            UpdateCameraRotation(delta);
        }

        void UpdateCameraTargetPosition()
        {
            var pos = Rig.transform.position + settings.CameraPositionOffset + shakeOffset;
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

        float duration = 1f;
        float force = 0.7f;
        float decreaseFactor = 1.0f;
        AnimationCurve shakeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        Vector3 shakeOffset;

        float time;

        public void ShakeOnAttack()
        {
            Shake(settings.cameraShakeOnAttack);
        }

        public void ShakeOnHit()
        {
            Shake(settings.cameraShakeOnHit);
        }

        void Shake(ShakeData data)
        {
            duration = data.duration;
            force = data.force;
            decreaseFactor = data.decreaseFactor;
            shakeCurve = data.curve;

            time = duration;
        }

        void UpdateShakeCamera(float delta)
        {
            if (time > 0)
            {
                var process = 1f - time / duration;
                var value = shakeCurve.Evaluate(process);

                shakeOffset = UnityEngine.Random.insideUnitSphere * force * value;
                time -= delta * decreaseFactor;
            }
            else
            {
                shakeOffset = Vector3.zero;
            }
        }
    }
}