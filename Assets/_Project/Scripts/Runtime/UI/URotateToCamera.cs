using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class URotateToCamera : MonoBehaviour
    {
        Camera cam;

        void Start() 
        {
            cam = Camera.main;
        }

        void LateUpdate()
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}