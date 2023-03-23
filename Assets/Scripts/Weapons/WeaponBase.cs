using System;
using System.Collections.Generic;
using Scriptables;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected WeaponInfo weaponInfo;
        [SerializeField] protected int damage;
        protected Rigidbody Rigidbody;
        private Collider _collider;
        protected bool CanPickup = true;
        public int CurrentAmount = 1;

        public event Action<WeaponBase> OnWeaponRemoved;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public abstract void AttackSingle();

        public abstract void AttackHold();

        public virtual void Pickup(Transform parent)
        {
            gameObject.SetActive(true);
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            Rigidbody.velocity = Vector3.zero;
            _collider.isTrigger = true;
            CanPickup = false;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void Drop()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
            _collider.isTrigger = false;
            CanPickup = true;
            OnWeaponRemoved?.Invoke(this);
        }

        public virtual bool TryAddToInventory(List<WeaponBase> inventory)
        {
            inventory.Add(this);
            return true;
        }

        public void SpawnNewWeapon(Transform parent, List<WeaponBase> inventory, ref WeaponBase current)
        {
            if (current.CurrentAmount >= 1)
            {
                var weapon = Instantiate(gameObject, parent.transform.position, Quaternion.identity, parent);

                if (weapon.TryGetComponent(out WeaponBase weaponBase))
                {
                    weaponBase.Pickup(parent);
                    inventory.Add(weaponBase);
                    inventory.Remove(current);
                    current = weaponBase;
                }
            }
        }

        public WeaponInfo GetWeaponInfo() => weaponInfo;

        public bool CanPickupWeapon() => CanPickup;
    }
}