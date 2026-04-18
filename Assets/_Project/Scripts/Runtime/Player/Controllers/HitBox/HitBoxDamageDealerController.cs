using System.Collections.Generic;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBoxDamageDealer : IHitBox
    {
        
    }

    public class HitBoxDamageDealerController : HitBoxController, IHitBoxDamageDealer
    {
        List<IHitBox> excludedHitBox;

        IAttacker attacker;

        protected override void OnInitialize()
        {
            attacker = GetComponentInParent<IAttacker>();

            excludedHitBox = new List<IHitBox>();

            var hitBoxes = GetComponentsInParent<IHitBox>();
            excludedHitBox.AddRange(hitBoxes);
        }

        protected override void PostInitialize()
        {
            SetHitBoxActive(false);
        }

        protected override void OnHitBoxTriggered(IHitBox hitBox)
        {
            if (excludedHitBox.Contains(hitBox))
            {
                return;
            }

            if (hitBox is IHitBoxReceiver receiver)
            {
                attacker.DamageUnit(receiver);
            }
        }
    }
}