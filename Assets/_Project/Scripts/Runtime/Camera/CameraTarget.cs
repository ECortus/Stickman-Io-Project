using UnityEngine;

namespace StickmanIo.Runtime
{
    public class CameraTarget : MonoBehaviour
    {
        public Transform Target => transform;
        
        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
        
        public Quaternion GetRotation()
        {
            return transform.rotation;
        }
        
        public Vector3 GetRotationAngles()
        {
            return transform.rotation.eulerAngles;
        }
        
        public void SetRotation(Vector3 angles)
        {
            SetRotation(Quaternion.Euler(angles));
        }
        
        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}