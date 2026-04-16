using System.Collections.Generic;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IHitBoxDamageDealer : IHitBox
    {
        void UpdateDamage(float damage, float mod = 1f);
    }

    public class HitBoxDamageDealerController : HitBoxController, IHitBoxDamageDealer
    {
        List<IHitBox> excludedHitBox;

        float damage = -1f;
        float damageMod = 1f;

        protected override void OnInitialize()
        {
            excludedHitBox = new List<IHitBox>();

            var hitBoxes = GetComponentsInParent<IHitBox>();
            excludedHitBox.AddRange(hitBoxes);
        }

        protected override void PostInitialize()
        {
            SetHitBoxActive(false);
        }

        public void UpdateDamage(float damage, float mod = 1f)
        {
            this.damage = damage;
            this.damageMod = mod;
        }

        protected override void OnHitBoxTriggered(IHitBox hitBox)
        {
            if (excludedHitBox.Contains(hitBox))
            {
                return;
            }

            if (hitBox is IHitBoxReceiver receiver)
            {
                var dmg = damage * damageMod;
                receiver.Damage(dmg);
            }
        }
    }
}