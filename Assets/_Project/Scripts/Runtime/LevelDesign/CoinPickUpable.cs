using System.Collections;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using GameDevUtils.Runtime.Triggers;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public class CoinPickUpable : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        [Space(10)] 
        [SerializeField] private int resourceAmount = 5;

        [Space(5)]
        [SerializeField] private float heightOffset = 1f;
        [SerializeField] private float speedMove = 5f;
        [SerializeField] private float speedRotate = 45f;
        [SerializeField] private float minDistance = 3f;

        Rigidbody rb;
    
        PlayerRig target;

        bool disabled = false;
        
        public void SetAmount(int customAmount)
        {
            resourceAmount = customAmount;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (target != null)
            {
                return;
            }

            if (other.IsSameMask(layerMask))
            {
                OnTriggerEnterMethod(other);
            }
        }

        void OnTriggerEnterMethod(Collider other)
        {
            if (other.TryGetComponent<PlayerRig>(out var playerRig))
            {
                SetPlayerTarget(playerRig);
            }
        }

        void SetPlayerTarget(PlayerRig playerRig)
        {
            target = playerRig;
            rb.isKinematic = true;
        }

        void DisableCoin()
        {
            disabled = true;
        }

        void Update()
        {
            if (disabled)
            {
                return;
            }

            transform.Rotate(Vector3.up * speedRotate * Time.deltaTime, Space.Self);

            if (!target)
            {
                return;
            }

            var targetPosition = target.transform.position;
            var destination = targetPosition + new Vector3(0, heightOffset, 0);

            if ((transform.position - destination).sqrMagnitude < minDistance * minDistance)
            {
                PickUpResource();
                return;
            }

            transform.position = Vector3.Slerp(transform.position, destination, speedMove * Time.deltaTime);
        }

        void PickUpResource()
        {
            DisableCoin();
            AddResource(target, resourceAmount);

            ObjectHelper.Destroy(this.gameObject);
        }

        void AddResource(PlayerRig rig, int amount)
        {
            var resources = rig.Resources;
            resources.AddCoins(amount);
        }

        public void Throw(Vector3 direction, float force)
        {
            rb.isKinematic = false;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}