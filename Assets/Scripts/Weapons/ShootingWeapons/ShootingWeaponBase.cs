using UnityEngine;

namespace Weapons.ShootingWeapons
{
    public class ShootingWeaponBase : WeaponBase
    {
        [SerializeField] private int clipSize, maxAmmo;
        private int _currentAmmo;

        protected override void Awake()
        {
            base.Awake();
            _currentAmmo = clipSize;
        }

        public virtual void Reload()
        {
        }
    }
}