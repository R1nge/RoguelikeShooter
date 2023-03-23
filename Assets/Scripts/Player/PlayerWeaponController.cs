using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Weapons.ShootingWeapons;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private Camera playerCamera;
        private WeaponBase _currentWeapon;
        private readonly List<WeaponBase> _weapons = new();
        private int _lastWeaponIndex;

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Raycast();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropWeapon(_currentWeapon);
            }

            if (_weapons.Count >= 1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SelectWeapon(0);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SelectWeapon(1);
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    _lastWeaponIndex = (_lastWeaponIndex + 1) % _weapons.Count;
                    SelectWeapon(_lastWeaponIndex);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _lastWeaponIndex = Mathf.Abs(_lastWeaponIndex - 1) % _weapons.Count;
                    SelectWeapon(_lastWeaponIndex);
                }
            }
        }

        private void Raycast()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance, layerMask))
            {
                if (hit.transform.TryGetComponent(out WeaponBase weapon))
                {
                    PickupWeapon(weapon);
                }
            }
        }

        private void Attack()
        {
            if (_currentWeapon)
            {
                _currentWeapon.Attack();
            }
        }

        private void Reload()
        {
            if (_currentWeapon.TryGetComponent(out ShootingWeaponBase shootingWeapon))
            {
                shootingWeapon.Reload();
            }
        }

        private void PickupWeapon(WeaponBase weapon)
        {
            if (weapon.TryAddToInventory(_weapons))
            {
                weapon.Pickup(weaponHolder);
                SelectWeapon(_weapons.Count - 1);
            }
            
        }

        private void DropWeapon(WeaponBase weapon)
        {
            _currentWeapon.OnWeaponDropped -= DropWeapon;
            weapon.Drop();
            weapon.RemoveFromInventory(_weapons, ref _currentWeapon);
        }

        private void SelectWeapon(int index)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.gameObject.SetActive(false);
                _currentWeapon.OnWeaponDropped -= DropWeapon;
            }

            _currentWeapon = _weapons[index % _weapons.Count];
            _currentWeapon.gameObject.SetActive(true);
            _currentWeapon.OnWeaponDropped += DropWeapon;
        }
    }
}