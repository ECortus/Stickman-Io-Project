using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBoxReceiver : IHitBox
    {
        void Damage(float damage);
        void Damage(float damage, out bool isKilled);
        void Damage(float damage, out bool isKilled, out IPlayerRig rig);
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
            health.TakeDamage(damage, out _, out _);
        }

        public void Damage(float damage, out bool isKilled)
        {
            health.TakeDamage(damage, out isKilled, out _);
        }

        public void Damage(float damage, out bool isKilled, out IPlayerRig rig)
        {
            health.TakeDamage(damage, out isKilled, out rig);
        }
    }
}