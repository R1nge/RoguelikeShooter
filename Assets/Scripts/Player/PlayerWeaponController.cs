using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private List<ShootingWeaponBase> weapons;
        private InputAction _weaponScroll;
        private InputAction _primaryAttackHold, _secondaryAttackHold;
        private int _currentWeaponIndex, _previousWeaponIndex;
        private AmmoUI _ammoUI;

        public Camera GetPlayerCamera() => playerCamera;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake()
        {
            _ammoUI = GetComponent<AmmoUI>();
            actions.FindActionMap("Player").FindAction("Interact").performed += Interact;
            actions.FindActionMap("Player").FindAction("PrimaryAttackSingle").performed += PrimaryAttackSingle;
            actions.FindActionMap("Player").FindAction("SecondaryAttackSingle").performed += SecondaryAttackSingle;
            _primaryAttackHold = actions.FindActionMap("Player").FindAction("PrimaryAttackHold");
            _secondaryAttackHold = actions.FindActionMap("Player").FindAction("SecondaryAttackHold");
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

            if (_primaryAttackHold.IsPressed() && !_secondaryAttackHold.IsPressed())
            {
                weapons[_currentWeaponIndex].AttackPrimaryHold();
            }

            if (!_primaryAttackHold.IsPressed() && _secondaryAttackHold.IsPressed())
            {
                weapons[_currentWeaponIndex].AttackSecondaryHold();
            }
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
                if (hit.transform.TryGetComponent(out ShootingWeaponBase weapon))
                {
                    PickupWeapon(weapon);
                }
            }
        }

        private void PickupWeapon(ShootingWeaponBase weapon)
        {
            weapon.AmmoAmountChangedEvent += UpdateAmmoUI;

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

        private void SwapWeapon(ShootingWeaponBase weapon)
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
            weapons[_currentWeaponIndex].AttackPrimarySingle();
        }

        private void SecondaryAttackSingle(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].AttackSecondarySingle();
        }

        private void StopAttack(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].StopAttack();
        }

        private void Reload(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            weapons[_currentWeaponIndex].Reload();
        }

        private void AddWeapon(ShootingWeaponBase weapon)
        {
            weapons.Add(weapon);
            SelectWeapon(weapons.Count - 1);
        }

        private void SelectWeapon(int index)
        {
            if (weapons.Count <= 0) return;
            var previous = weapons[_currentWeaponIndex];
            if (previous != null)
            {
                previous.AmmoAmountChangedEvent -= UpdateAmmoUI;
                previous.gameObject.SetActive(false);
            }

            _previousWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex = index;
            var current = weapons[_currentWeaponIndex];
            current.AmmoAmountChangedEvent += UpdateAmmoUI;
            current.gameObject.SetActive(true);
        }

        private void UpdateAmmoUI(int ammo, int maxAmmo, bool isInfinite)
        {
            _ammoUI.UpdateUI(ammo, maxAmmo, isInfinite);
        }

        private bool HasWeapon()
        {
            if (weapons.Count <= 0) return false;
            if (weapons[_currentWeaponIndex] == null) return false;
            return true;
        }
    }
}