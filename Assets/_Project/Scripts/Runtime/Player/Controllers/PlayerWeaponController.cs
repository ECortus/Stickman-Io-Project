using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface IPlayerWeaponController
    {
        bool WeaponActive { get; }

        void SetCollidersActive(bool active);
        void SetWeaponActive(bool active);

        void SetUpdatedDamage(float damage, float mod = 1f);
    }

    public class PlayerWeaponController : MonoBehaviour, IPlayerWeaponController
    {
        [SerializeField] private bool instantiateWeapon = false;
        [SerializeField] private Transform weapon;

        [Space(5)]
        [SerializeField] private Transform inHandParent;
        [SerializeField] private Transform behindParent;

        [Space(5)]
        [SerializeField] private float overrideScale = 0.75f;

        bool initialized = false;

        GameObject instance;
        IHitBoxDamageDealer hitBox;

        public bool WeaponActive { get; private set; }

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            if (initialized)
            {
                return;
            }

            if (instantiateWeapon)
            {
                instance = ObjectInstantiator.InstantiatePrefab(weapon, behindParent);
            }
            else
            {
                instance = weapon.gameObject;
            }

            hitBox = instance.GetComponentInChildren<IHitBoxDamageDealer>();
            initialized = true;
        }

        public void SetUpdatedDamage(float damage, float mod = 1f)
        {
            if (!initialized)
            {
                Initialize();
            }

            hitBox.UpdateDamage(damage, mod);
        }

        public void SetCollidersActive(bool active)
        {
            if (!initialized)
            {
                Initialize();
            }

            hitBox.SetHitBoxActive(active);
        }

        public void SetWeaponActive(bool active)
        {
            if (!initialized)
            {
                Initialize();
            }

            if (active)
            {
                instance.transform.SetParent(inHandParent);

                instance.transform.ResetAllLocalParameters();
                instance.transform.localScale = Vector3.one * overrideScale;

                WeaponActive = true;
            }
            else
            {
                instance.transform.SetParent(behindParent);

                instance.transform.ResetAllLocalParameters();
                instance.transform.localScale = Vector3.one * overrideScale;

                WeaponActive = false;
            }
        }
    }
}