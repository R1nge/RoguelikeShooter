using System;
using Damageable;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Kunai : WeaponBase
    {
        [SerializeField] private float force;

        public override void Attack()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.AddForce(-transform.forward * force, ForceMode.Impulse);
        }

        public override void Pickup(Transform parent)
        {
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}