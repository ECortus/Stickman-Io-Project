using GameDevUtils.Runtime.Extensions;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime
{
    public class HitBoxDamageDealerSimple : MonoBehaviour
    {
        [SerializeField] private float damageInEnter = 25f;
        [SerializeField] private float damageInStay = 0.5f;
        [SerializeField] private LayerMask hitBoxLayerMask;

        void OnTriggerEnter(Collider other)
        {
            if (other.IsSameMask(hitBoxLayerMask))
            {
                var hitBox = other.GetComponentInParent<IHitBox>();
                if (hitBox is IHitBoxReceiver receiver)
                {
                    receiver.Damage(damageInEnter);
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.IsSameMask(hitBoxLayerMask))
            {
                var hitBox = other.GetComponentInParent<IHitBox>();
                if (hitBox is IHitBoxReceiver receiver)
                {
                    receiver.Damage(damageInStay);
                }
            }
        }
    }
}