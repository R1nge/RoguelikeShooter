using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Weapons.ShootingWeapons;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private Camera playerCamera;
        private WeaponBase _currentWeapon;
        private List<WeaponBase> _weapons = new();

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickupWeapon();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropWeapon(_currentWeapon);
            }
        }

        private void PickupWeapon()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out WeaponBase weapon))
                {
                    AddWeapon(weapon);
                    PickupWeapon(weapon);
                }
            }
        }

        private void AddWeapon(WeaponBase weapon)
        {
            if (_currentWeapon == weapon)
            {
                Debug.LogError("Pickuped the same weapon");
                return;
            }

            _currentWeapon = weapon;
            _weapons.Add(weapon);
        }

        private void RemoveWeapon(WeaponBase weapon)
        {
            _weapons.Remove(weapon);
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
            weapon.Pickup(weaponHolder);
        }

        private void DropWeapon(WeaponBase weapon)
        {
            RemoveWeapon(weapon);
            _currentWeapon = null;
            weapon.Drop();
        }
    }
}