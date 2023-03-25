using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private List<WeaponBase> weapons;
        private InputAction _weaponScroll;
        private int _currentWeaponIndex, _previousWeaponIndex;

        public Camera GetPlayerCamera() => playerCamera;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake()
        {
            actions.FindActionMap("Player").FindAction("Interact").performed += Interact;
            actions.FindActionMap("Player").FindAction("PrimaryAttackSingle").performed += PrimaryAttackSingle;
            actions.FindActionMap("Player").FindAction("PrimaryAttackHold").performed += PrimaryAttackHold;
            actions.FindActionMap("Player").FindAction("StopAttack").performed += StopAttack;
            actions.FindActionMap("Player").FindAction("Reload").performed += Reload;
            _weaponScroll = actions.FindActionMap("Player").FindAction("WeaponScroll");
            SelectWeapon(0);
            weapons[_currentWeaponIndex].SetOwner(this);
        }

        private void OnDestroy()
        {
            actions.FindActionMap("Player").FindAction("Interact").performed -= Interact;
            actions.FindActionMap("Player").FindAction("PrimaryAttackSingle").performed -= PrimaryAttackSingle;
            actions.FindActionMap("Player").FindAction("PrimaryAttackHold").performed -= PrimaryAttackHold;
            actions.FindActionMap("Player").FindAction("StopAttack").performed -= StopAttack;
            actions.FindActionMap("Player").FindAction("Reload").performed -= Reload;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Raycast();
        }

        private void Update()
        {
            WeaponScroll();
        }

        private void WeaponScroll()
        {
            if (weapons.Count >= 1)
            {
                var scroll = _weaponScroll.ReadValue<float>();
                if (scroll > 0)
                {
                    SelectWeapon((_currentWeaponIndex + 1) % weapons.Count);
                }
                else if (scroll < 0)
                {
                    var index = (_currentWeaponIndex - 1) % weapons.Count;
                    if (index < 0)
                    {
                        index = weapons.Count - 1;
                    }

                    SelectWeapon(index);
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

        private void PickupWeapon(WeaponBase weapon)
        {
            if (weapons.Count == 3)
            {
                SwapWeapon(weapon);
            }
            else
            {
                weapon.Pickup(weaponHolder, this);
                AddWeapon(weapon);
            }
        }

        private void SwapWeapon(WeaponBase weapon)
        {
            if (_currentWeaponIndex != 0)
            {
                weapons[_currentWeaponIndex].Drop();
                weapons[_currentWeaponIndex] = weapon;
                SelectWeapon(_currentWeaponIndex);
            }
            else
            {
                weapons[_previousWeaponIndex].Drop();
                weapons[_previousWeaponIndex] = weapon;
                SelectWeapon(_previousWeaponIndex);
            }
            
            weapon.Pickup(weaponHolder, this);
        }

        private void PrimaryAttackSingle(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].AttackSingle();
        }

        private void PrimaryAttackHold(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].AttackHold();
        }

        private void StopAttack(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].StopAttack();
        }

        private void Reload(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            var weapon = (ShootingWeaponBase)weapons[_currentWeaponIndex];
            weapon.Reload();
        }

        private void AddWeapon(WeaponBase weapon)
        {
            weapons.Add(weapon);
            SelectWeapon(weapons.Count - 1);
        }

        private void SelectWeapon(int index)
        {
            if (weapons.Count <= 0) return;
            if (weapons[_currentWeaponIndex] != null)
            {
                weapons[_currentWeaponIndex].gameObject.SetActive(false);
            }
            
            _previousWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex = index;
            weapons[_currentWeaponIndex].gameObject.SetActive(true);
        }

        private bool HasWeapon()
        {
            if (weapons.Count <= 0) return false;
            if (weapons[_currentWeaponIndex] == null) return false;
            return true;
        }
    }
}