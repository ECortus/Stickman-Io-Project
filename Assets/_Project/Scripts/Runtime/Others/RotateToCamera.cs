using UnityEngine;

namespace StickmanIo.Runtime
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class RotateToCamera : MonoBehaviour
    {
        Camera cam;

        void Start() 
        {
            cam = Camera.main;
        }

        void LateUpdate()
        {
            if (cam == null)
            {
                return;
            }

            transform.rotation = cam.transform.rotation;
        }
    }
}