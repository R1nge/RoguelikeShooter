using System.Collections.Generic;
using System.Linq;
using Damageable;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class ThrowableWeaponBase : WeaponBase
    {
        [SerializeField] protected int maxAmount;
        [SerializeField] protected float force;

        public override void AttackSingle()
        {
            Drop();
            Rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        }

        public override void AttackHold()
        {
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (Rigidbody.velocity.magnitude <= 5) return;
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

        public override void RemoveFromInventory(List<WeaponBase> inventory, ref WeaponBase current)
        {
            if (CurrentAmount > 1)
            {
                CurrentAmount = 1;
            }
            else
            {
                inventory.Remove(current);
                current = null;
            }
        }

        public override bool TryAddToInventory(List<WeaponBase> inventory)
        {
            if (inventory.Any(weapon => weapon.GetWeaponInfo().weaponName == GetWeaponInfo().weaponName))
            {
                var weapon = inventory.First(weapon => weapon.GetWeaponInfo().weaponName == GetWeaponInfo().weaponName);
                if (weapon.CurrentAmount < maxAmount)
                {
                    weapon.CurrentAmount++;
                    Destroy(gameObject);
                    return true;
                }

                return false;
            }

            inventory.Add(this);
            return true;
        }
    }
}