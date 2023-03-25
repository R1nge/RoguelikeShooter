using System.Collections;
using Animators;
using Damageable;
using Player;
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
        protected bool CanShoot = true;
        protected int CurrentAmmoAmount;
        protected ShootingWeaponAnimatorControllerBase ShootingWeaponAnimatorControllerBase;
        private Coroutine _reloadCoroutine;
        private float _nextFire;

        protected override void Awake()
        {
            base.Awake();
            ShootingWeaponAnimatorControllerBase = GetComponent<ShootingWeaponAnimatorControllerBase>();
            CurrentAmmoAmount = clipSize;
        }

        public override void AttackHold()
        {
            if (CurrentAmmoAmount <= 0)
            {
                Reload();
            }
            else
            {
                if (CanShoot)
                {
                    Shoot();
                }
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
            ShootingWeaponAnimatorControllerBase.Reload();
            yield return new WaitForSeconds(reloadTime);
            ShootingWeaponAnimatorControllerBase.StopReload();
            CurrentAmmoAmount = clipSize;
            CanShoot = true;
            _reloadCoroutine = null;
        }

        private void Shoot()
        {
            if (Time.time > _nextFire)
            {
                _nextFire = Time.time + 1 / (fireRate / 60);
                Raycast();
                ShootingWeaponAnimatorControllerBase.AttackHold();
                CurrentAmmoAmount--;
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
            if (CurrentAmmoAmount <= 0)
            {
                _reloadCoroutine = StartCoroutine(Reload_c());
            }
            else
            {
                CanShoot = true;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _reloadCoroutine = null;
        }
    }
}