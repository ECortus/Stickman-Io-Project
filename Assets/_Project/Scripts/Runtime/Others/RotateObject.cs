using UnityEngine;

namespace StickmanIo.Runtime.Others
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 0f, 0f);

        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}