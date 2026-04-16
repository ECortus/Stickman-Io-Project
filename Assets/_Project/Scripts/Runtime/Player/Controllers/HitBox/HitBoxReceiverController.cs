using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBoxReceiver : IHitBox
    {
        void Damage(float damage);
    }

    public class HitBoxReceiverController : HitBoxController, IHitBoxReceiver
    {
        IHealth health;

        protected override void OnInitialize()
        {
            health = GetComponentInParent<IHealth>();
        }

        public void Damage(float damage)
        {
            health.TakeDamage(damage);
        }
    }
}