using Damageable;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class ThrowableWeaponBase : WeaponBase
    {
        [SerializeField] protected int maxAmount;
        [SerializeField] protected float force;
        protected int CurrentAmount;

        public override void Attack()
        {
            Drop();
            Rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (Rigidbody.velocity.magnitude <= 5) return;
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

        //TODO: add weapon stacking
    }
}