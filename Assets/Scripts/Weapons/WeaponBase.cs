using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected int damage;
        protected Rigidbody Rigidbody;

        protected virtual void Awake() => Rigidbody = GetComponent<Rigidbody>();

        public abstract void Attack();

        public virtual void Pickup(Transform parent)
        {
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void Drop()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
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