using System.Collections.Generic;
using System.Linq;
using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using PurrNet;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.LevelDesign
{
    public abstract class PickupAbleObject : NetworkIdentity
    {
        [SerializeField] private LayerMask layerMask;

        [Space(10)]
        [SerializeField] private int resourceAmount = 5;

        [Space(5)]
        [SerializeField] private float heightOffset = 1f;
        [SerializeField] private float speedMove = 5f;
        [SerializeField] private float speedRotate = 45f;
        [SerializeField] private float minDistance = 3f;

        [Space(5)]
        [SerializeField] private float delayBeforeCanPickup = 1f;

        Rigidbody rb;

        PlayerRig target;

        List<EntityId> entityIds = new List<EntityId>();

        float delay = 0;
        bool disabled = false;

        public void SetDelayBeforeCanPickup()
        {
            delay = delayBeforeCanPickup;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        void OnTriggerStay(Collider other)
        {
            if (delay > 0)
            {
                return;
            }

            if (target)
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
            if (entityIds.Find(c => c == other.GetEntityId()).IsValid())
            {
                return;
            }

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

        void DisableObject()
        {
            disabled = true;
        }

        void Update()
        {
            float delta = Time.deltaTime;
            delay -= delta;

            if (disabled)
            {
                return;
            }

            if (!rb.isKinematic)
            {
                rb.angularVelocity = Vector3.zero;
            }

            var eulers = Vector3.zero;
            eulers.y = transform.rotation.eulerAngles.y + speedRotate * delta;
            rb.MoveRotation(Quaternion.Euler(eulers));

            if (!target)
            {
                return;
            }

            var targetPosition = target.transform.position;
            var destination = targetPosition + new Vector3(0, heightOffset, 0);

            if ((rb.position - destination).sqrMagnitude < minDistance * minDistance)
            {
                PickUpResource();
                return;
            }

            var newPosition = Vector3.MoveTowards(rb.position, destination, speedMove * delta);
            rb.MovePosition(newPosition);
        }

        void PickUpResource()
        {
            DisableObject();
            AddResource(target, resourceAmount);

            ObjectHelper.Destroy(this.gameObject);
        }

        protected abstract void AddResource(PlayerRig rig, int amount);

        public void SetAmount(int customAmount)
        {
            resourceAmount = customAmount;
        }

        public void SetExcludedEntities(EntityId id, bool addToExsited = false)
        {
            SetExcludedEntities(new EntityId[] { id }, addToExsited);
        }

        public void SetExcludedEntities(EntityId[] ids, bool addToExsited = false)
        {
            if (addToExsited)
            {
                entityIds.AddRange(ids);
            }
            else
            {
                entityIds = ids.ToList();
            }
        }

        public void Throw(Vector3 direction, float force)
        {
            rb.isKinematic = false;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}