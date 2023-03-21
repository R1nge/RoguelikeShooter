using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected int damage;
        protected Rigidbody Rigidbody;
        private Collider _collider;

        //TODO: redo
        public event Action<WeaponBase> OnWeaponDropped;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public abstract void Attack();

        public virtual void Pickup(Transform parent)
        {
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            _collider.isTrigger = true;
        }

        public virtual void Drop()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
            OnWeaponDropped?.Invoke(this);
            _collider.isTrigger = false;
        }

        public void RemoveFromInventory(List<WeaponBase> inventory, ref WeaponBase current)
        {
            current = null;
            inventory.Remove(this);
        }

        public virtual void AddToInventory(List<WeaponBase> inventory)
        {
            inventory.Add(this);
        }
    }
}