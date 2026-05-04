using UnityEngine;
using PurrNet;

namespace StickmanIo.Runtime.Others
{
    public class DeleteAllCollidersAndRigidbodies : MonoBehaviour
    {
        #if UNITY_EDITOR

        [ContextMenu("Destroy All Colliders and Rigidbodies")]
        void DestroyMethod()
        {
            foreach (var joint in GetComponentsInChildren<CharacterJoint>())
            {
                DestroyImmediate(joint);
            }

            foreach (var netRb in GetComponentsInChildren<NetworkRigidbody>())
            {
                DestroyImmediate(netRb);
            }

            foreach (var netTr in GetComponentsInChildren<NetworkTransform>())
            {
                DestroyImmediate(netTr);
            }

            foreach (var rigidbody in GetComponentsInChildren<Rigidbody>())
            {
                DestroyImmediate(rigidbody);
            }
            
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                DestroyImmediate(collider);
            }

            UnityEditor.EditorUtility.SetDirty(this.gameObject);
        }

        #endif
    }
}