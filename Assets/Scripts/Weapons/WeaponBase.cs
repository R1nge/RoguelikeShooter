using Animators;
using Player;
using Scriptables;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] private Vector3 positionOffset, rotationOffset;
        [SerializeField] protected WeaponInfo weaponInfo;
        protected bool CanPickup = true;
        protected Rigidbody Rigidbody;
        protected Collider Collider;
        protected WeaponAnimatorControllerBase WeaponAnimatorControllerBase;
        protected PlayerWeaponController Owner;

        public WeaponInfo GetWeaponInfo() => weaponInfo;
        public bool CanPickupWeapon() => CanPickup;
        public void SetOwner(PlayerWeaponController owner) => Owner = owner;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
            WeaponAnimatorControllerBase = GetComponent<WeaponAnimatorControllerBase>();
        }

        public virtual void AttackSingle()
        {
            WeaponAnimatorControllerBase.AttackSingle();
        }

        public virtual void AttackHold()
        {
            WeaponAnimatorControllerBase.AttackHold();
        }

        public virtual void StopAttack()
        {
            WeaponAnimatorControllerBase.StopAttack();
        }

        public virtual void Pickup(Transform parent, PlayerWeaponController owner)
        {
            if (!CanPickup) return;
            gameObject.SetActive(true);
            CanPickup = false;
            transform.parent = parent;
            Rigidbody.isKinematic = true;
            Collider.isTrigger = true;
            SetOwner(owner);
            transform.SetLocalPositionAndRotation(positionOffset, Quaternion.Euler(rotationOffset));
        }

        public virtual void Drop()
        {
            gameObject.SetActive(true);
            CanPickup = true;
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
            RemoveFromInventory();
        }

        protected void RemoveFromInventory()
        {
            SetOwner(null);
        }
    }
}