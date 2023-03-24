using Damageable;
using Player;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Kunai : WeaponBase
    {
        [SerializeField] protected float throwForce;
        [SerializeField] protected int damage;
        private bool _canBounce;

        public override void AttackSingle()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.AddForce(-transform.forward * throwForce, ForceMode.Impulse);
            Collider.isTrigger = false;
            _canBounce = true;
            RemoveFromInventory();
            CanPickup = true;
        }

        public override void Pickup(Transform parent, PlayerWeaponController owner)
        {
            base.Pickup(parent, owner);
            _canBounce = false;
        }

        public override void Drop()
        {
            base.Drop();
            _canBounce = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_canBounce)
            {
                var dir = Vector3.Reflect(Rigidbody.velocity, other.contacts[0].normal);
                Rigidbody.AddForce(dir * throwForce, ForceMode.Impulse);
            }

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}