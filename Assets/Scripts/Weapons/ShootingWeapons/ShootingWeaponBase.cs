using System.Collections;
using Animators;
using Damageable;
using UnityEngine;

namespace Weapons.ShootingWeapons
{
    public class ShootingWeaponBase : WeaponBase
    {
        [SerializeField] protected int damage;
        [SerializeField] protected float shootDistance;
        [SerializeField] protected int maxAmmoAmount, clipSize;

        [Tooltip("Fire rate per minute")] [SerializeField]
        protected float fireRate;

        [SerializeField] protected float reloadTime;
        [SerializeField] protected LayerMask hitLayer;
        private bool _canShoot = true;
        private int _currentAmmoAmount;
        private float _nextFire;
        private ShootingWeaponAnimatorControllerBase _shootingWeaponAnimatorControllerBase;
        private int _totalAmmo;
        private Coroutine _reloadCoroutine;

        protected override void Awake()
        {
            base.Awake();
            _shootingWeaponAnimatorControllerBase = GetComponent<ShootingWeaponAnimatorControllerBase>();
            _currentAmmoAmount = clipSize;
            _totalAmmo = maxAmmoAmount;
        }

        public override void AttackHold()
        {
            if (_currentAmmoAmount <= 0)
            {
                Reload();
                _shootingWeaponAnimatorControllerBase.StopAttack();
                _canShoot = false;
            }
            else
            {
                if (_canShoot)
                {
                    Shoot();
                    print("SHOOOOOT");
                }
            }
        }

        public void Reload()
        {
            if (_reloadCoroutine != null) return;
            if (_totalAmmo == 0) return;
            if (_currentAmmoAmount == clipSize) return;
            _reloadCoroutine = StartCoroutine(Reload_c());
        }

        private IEnumerator Reload_c()
        {
            _canShoot = false;
            _shootingWeaponAnimatorControllerBase.Reload();
            yield return new WaitForSeconds(reloadTime);
            _shootingWeaponAnimatorControllerBase.StopReload();

            int ammoToReload = clipSize - _currentAmmoAmount;
            if (ammoToReload < _totalAmmo)
            {
                _totalAmmo -= ammoToReload;
                _currentAmmoAmount += ammoToReload;
            }
            else
            {
                _currentAmmoAmount += _totalAmmo;
                _totalAmmo = 0;
            }

            _canShoot = true;
            _reloadCoroutine = null;
        }

        private void Shoot()
        {
            if (Time.time > _nextFire)
            {
                _nextFire = Time.time + 1 / (fireRate / 60);
                Raycast();
                _shootingWeaponAnimatorControllerBase.AttackHold();
                _currentAmmoAmount--;
            }
        }

        private void Raycast()
        {
            var cameraTransform = Owner.GetPlayerCamera().transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out var hit, shootDistance, hitLayer))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
        }

        private void OnEnable()
        {
            if (_currentAmmoAmount <= 0)
            {
                _reloadCoroutine = StartCoroutine(Reload_c());
            }
            else
            {
                _canShoot = true;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _reloadCoroutine = null;
        }
    }
}