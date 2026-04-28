using UnityEditor;
using UnityEngine;
using PurrNet;

namespace StickmanIo.Runtime.Player
{
    public class RagdollController : NetworkIdentity
    {
        [Header("Ragdool parameters")]
        [SerializeField] private Vector3 forceMoveDirection = Vector3.up;
        [SerializeField] private float ragdollForce = 100f;
        [SerializeField] private float ragdollTorque = 100f;

        [Header("Default parameters and object")]
        [SerializeField] private Transform bodiesParent;
        [SerializeField] private bool alreadyBaked = false;
        [SerializeField] private Rigidbody[] rigidbodies;
        [SerializeField] private Collider[] colliders;

        [SerializeField] private Vector3[] defaultPositions;
        [SerializeField] private Quaternion[] defaultRotations;

        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            if (!alreadyBaked)
            {
                BakeRigidbodiesAndColliders();
            }

            OffRagdoll();
        }

        [ContextMenu("Bake Rigidbodies and Colliders")]
        public void BakeRigidbodiesAndColliders()
        {
            alreadyBaked = true;

            rigidbodies = bodiesParent.GetComponentsInChildren<Rigidbody>(true);
            colliders = bodiesParent.GetComponentsInChildren<Collider>(true);

            defaultPositions = new Vector3[rigidbodies.Length];
            defaultRotations = new Quaternion[rigidbodies.Length];

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                defaultPositions[i] = rigidbodies[i].transform.localPosition;
                defaultRotations[i] = rigidbodies[i].transform.localRotation;
            }

            OffRagdoll();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void SetToRagdoll()
        {
            animator.enabled = false;

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].isKinematic = false;
                colliders[i].enabled = true;
            }

            var force = forceMoveDirection * ragdollForce;

            var torqueDirection = Random.insideUnitSphere;
            var torque = torqueDirection * ragdollTorque;

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].AddForce(force, ForceMode.Impulse);
                rigidbodies[i].AddTorque(torque, ForceMode.Impulse);
            }
        }

        public void OffRagdoll()
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].isKinematic = true;
                colliders[i].enabled = false;

                rigidbodies[i].transform.localPosition = defaultPositions[i];
                rigidbodies[i].transform.localRotation = defaultRotations[i];
            }
        }
    }
}