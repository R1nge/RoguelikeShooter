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
        private List<WeaponBase> _weapons = new();
        private int _currentWeaponIndex;

        public List<WeaponBase> GetWeapons() => _weapons;

        public void SelectLastWeapon()
        {
            _currentWeaponIndex = _weapons.Count;
            if (_currentWeaponIndex - 1 < 0)
            {
                _currentWeaponIndex = 0;
            }
            else
            {
                _currentWeaponIndex = _weapons.Count - 1;
            }
            
            SelectWeapon(_currentWeaponIndex);
        }

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (_weapons.Count > 0)
            {
                if (_weapons[_currentWeaponIndex] != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        AttackSingle();
                    }

                    if (Input.GetMouseButton(0))
                    {
                        AttackHold();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        Reload();
                    }

                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        DropWeapon();
                    }
                }

                if (_weapons.Count >= 1)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        SelectWeapon((_currentWeaponIndex + 1) % _weapons.Count);
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        SelectWeapon(Mathf.Abs(_currentWeaponIndex - 1) % _weapons.Count);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Raycast();
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

        private void AttackSingle()
        {
            _weapons[_currentWeaponIndex]?.AttackSingle();
        }

        private void AttackHold()
        {
            //TODO: fix
            _weapons[_currentWeaponIndex]?.AttackHold();
        }

        private void Reload()
        {
            var weapon = (ShootingWeaponBase)_weapons[_currentWeaponIndex];
            weapon.Reload();
        }

        private void PickupWeapon(WeaponBase weapon)
        {
            weapon.Pickup(weaponHolder, this);
            AddWeapon(weapon);
        }

        private void AddWeapon(WeaponBase weapon)
        {
            _weapons.Add(weapon);
            SelectWeapon(_weapons.Count - 1);
        }

        private void DropWeapon()
        {
            _weapons[_currentWeaponIndex]?.Drop();
        }

        private void SelectWeapon(int index)
        {
            if (_weapons.Count <= 0) return;
            if (_weapons[_currentWeaponIndex] != null)
            {
                _weapons[_currentWeaponIndex].gameObject.SetActive(false);
            }

            _currentWeaponIndex = index;
            _weapons[_currentWeaponIndex].gameObject.SetActive(true);
        }
    }
}