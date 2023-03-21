using System;
using System.Collections;
using Damageable;
using UnityEngine;

namespace Weapons.ShootingWeapons
{
    public abstract class ShootingWeaponBase : WeaponBase
    {
        [SerializeField] private float shootDistance;
        [SerializeField] protected int  maxAmmoAmount, clipSize;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float reloadTime;
        [SerializeField] protected Transform shootPoint;
        protected bool CanShoot = true;
        protected int CurrentAmmoAmount;
        private Coroutine _reloadCoroutine;

        protected override void Awake()
        {
            base.Awake();
            CurrentAmmoAmount = clipSize;
        }

        public override void Attack()
        {
            if (CurrentAmmoAmount <= 0)
            {
                Reload();
            }
            else
            {
                Shoot();
            }
        }

        public void Reload()
        {
            if (_reloadCoroutine != null) return;
            _reloadCoroutine = StartCoroutine(Reload_c());
        }

        private IEnumerator Reload_c()
        {
            CanShoot = false;
            yield return new WaitForSeconds(reloadTime);
            CurrentAmmoAmount = clipSize;
            CanShoot = true;
            _reloadCoroutine = null;
        }

        private void Raycast()
        {
            Ray ray = new Ray(shootPoint.position, shootPoint.forward);
            if (Physics.Raycast(ray, out var hit, shootDistance))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
        }

        private void Shoot()
        {
            Raycast();
            CurrentAmmoAmount--;
        }

        private void OnEnable()
        {
            if (!CanShoot)
            {
                _reloadCoroutine = StartCoroutine(Reload_c());
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}