using System;
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
        [SerializeField] protected int clipSize;

        [Tooltip("Fire rate per minute"), SerializeField]
        protected float fireRate;

        [SerializeField] protected float reloadTime;
        [SerializeField] protected LayerMask hitLayer;
        [SerializeField] protected bool isInfinite;
        private bool _canShoot = true;
        private int _currentAmmoAmount;
        [SerializeField, HideInInspector] private int _maxAmmoAmount;
        private float _nextFire;
        private ShootingWeaponAnimatorControllerBase _shootingWeaponAnimatorControllerBase;
        private int _totalAmmo;
        private Coroutine _reloadCoroutine;

        private int CurrentAmmoAmount
        {
            get => _currentAmmoAmount;
            set
            {
                _currentAmmoAmount = value;
                AmmoAmountChangedEvent?.Invoke(_currentAmmoAmount, _totalAmmo, isInfinite);
            }
        }

        public event Action<int, int, bool> AmmoAmountChangedEvent;

        public bool IsInfinite() => isInfinite;

        public int GetMaxAmmoAmount() => _maxAmmoAmount;

        public void SetMaxAmmoAmount(int value) => _maxAmmoAmount = value;

        protected override void Awake()
        {
            base.Awake();
            _shootingWeaponAnimatorControllerBase = GetComponent<ShootingWeaponAnimatorControllerBase>();
            CurrentAmmoAmount = clipSize;
            _totalAmmo = _maxAmmoAmount;
        }

        private void Start()
        {
            AmmoAmountChangedEvent?.Invoke(_currentAmmoAmount, _totalAmmo, isInfinite);
        }

        public override void AttackHold()
        {
            if (CurrentAmmoAmount <= 0)
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
                }
            }
        }

        public void Reload()
        {
            if (CurrentAmmoAmount == clipSize) return;

            if (!isInfinite)
            {
                if (_totalAmmo == 0) return;
                if (_reloadCoroutine != null) return;
            }

            _reloadCoroutine = StartCoroutine(Reload_c());
        }

        private IEnumerator Reload_c()
        {
            _canShoot = false;
            _shootingWeaponAnimatorControllerBase.Reload();
            yield return new WaitForSeconds(reloadTime);
            _shootingWeaponAnimatorControllerBase.StopReload();

            if (isInfinite)
            {
                CurrentAmmoAmount = clipSize;
            }
            else
            {
                int ammoToReload = clipSize - CurrentAmmoAmount;
                if (ammoToReload < _totalAmmo)
                {
                    _totalAmmo -= ammoToReload;
                    CurrentAmmoAmount += ammoToReload;
                }
                else
                {
                    CurrentAmmoAmount += _totalAmmo;
                    _totalAmmo = 0;
                }
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

        public override void Pickup(Transform parent, PlayerWeaponController owner)
        {
            base.Pickup(parent, owner);
            AmmoAmountChangedEvent?.Invoke(_currentAmmoAmount, _totalAmmo, isInfinite);
        }

        private void OnEnable()
        {
            if (CurrentAmmoAmount <= 0)
            {
                _reloadCoroutine = StartCoroutine(Reload_c());
            }
            else
            {
                _canShoot = true;
            }
            
            AmmoAmountChangedEvent?.Invoke(_currentAmmoAmount, _totalAmmo, isInfinite);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _reloadCoroutine = null;
        }
    }
}