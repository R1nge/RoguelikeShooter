using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
            if (_currentWeapon == null)
            {
                _currentWeapon = weapon;
            }

            weapon.Pickup(weaponHolder);
            weapon.AddToInventory(_weapons);
        }

        private void DropWeapon(WeaponBase weapon)
        {
            weapon.Drop();
            //weapon.RemoveFromInventory(_weapons, ref _currentWeapon);
        }

        //TODO: add weapon scroll
    }
}