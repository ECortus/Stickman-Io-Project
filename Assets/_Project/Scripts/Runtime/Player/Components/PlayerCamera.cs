using Unity.Cinemachine;
using CameraTarget = StickmanIo.Runtime.Camera.CameraTarget;

namespace StickmanIo.Runtime.Player
{
    public class PlayerCamera : RigComponent
    {
        protected override void OnInitialize()
        {
            var targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
            var cameraTarget = GetComponentInChildren<CameraTarget>();
            
            targetGroup.Targets.Clear();
            targetGroup.AddMember(cameraTarget.Target, 1f, 1f);
        }
    }
}