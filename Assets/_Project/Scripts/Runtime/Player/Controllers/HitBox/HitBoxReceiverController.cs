using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBoxReceiver : IHitBox
    {
        void Damage(float damage);
    }

    public class HitBoxReceiverController : HitBoxController, IHitBoxReceiver
    {
        public void Damage(float damage)
        {
            Debug.Log("Damage received by " + gameObject.name + " by " + damage + "!");  
        }
    }
}