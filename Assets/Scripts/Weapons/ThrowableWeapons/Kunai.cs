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

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                if (_canBounce)
                {
                    var dir = Vector3.Reflect(other.contacts[0].point - transform.position, other.contacts[0].normal);
                    Rigidbody.AddForce(dir * throwForce, ForceMode.Impulse);
                    _canBounce = false;
                }

                damageable.TakeDamage(damage);
            }
        }
    }
}