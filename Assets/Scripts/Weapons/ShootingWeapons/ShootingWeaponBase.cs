using UnityEngine;

namespace Weapons.ShootingWeapons
{
    public abstract class ShootingWeaponBase : WeaponBase
    {
        [SerializeField] protected int currentAmmoAmount, maxAmmoAmount, clipSize;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float reloadTime;
        
        public override void Attack()
        {
            print("Shooting weapon base: attack");
        }

        public virtual void Reload()
        {
        }
    }
}