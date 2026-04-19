using UnityEngine;

namespace StickmanIo.Runtime
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class FollowToTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = target.position + offset;
        }
    }
}