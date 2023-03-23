using System.Collections.Generic;
using Player;
using Scriptables;
using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected WeaponInfo weaponInfo;
        protected bool CanPickup = true;
        protected Rigidbody Rigidbody;
        protected Collider Collider;
        protected PlayerWeaponController Owner;

        public WeaponInfo GetWeaponInfo() => weaponInfo;
        public bool CanPickupWeapon() => CanPickup;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }

        public virtual void AttackSingle()
        {
        }

        public virtual void AttackHold()
        {
        }

        public virtual void Pickup(Transform parent, PlayerWeaponController owner)
        {
            if (!CanPickup) return;
            CanPickup = false;
            transform.parent = parent;
            Rigidbody.isKinematic = true;
            Collider.isTrigger = true;
            Owner = owner;
        }

        public virtual void Drop()
        {
            CanPickup = true;
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
            RemoveFromInventory();
        }

        public void RemoveFromInventory()
        {
            //TODO: redo
            Owner.GetWeapons().Remove(this);
            Owner.SelectLastWeapon();
            Owner = null;
        }
    }
}