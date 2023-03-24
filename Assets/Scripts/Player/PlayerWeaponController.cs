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
        private InputAction _weaponScroll;
        private readonly List<WeaponBase> _weapons = new();
        private int _currentWeaponIndex;

        public Camera GetPlayerCamera() => playerCamera;

        public List<WeaponBase> GetWeapons() => _weapons;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake()
        {
            actions.FindActionMap("Player").FindAction("Interact").performed += Interact;
            actions.FindActionMap("Player").FindAction("PrimaryAttackSingle").performed += PrimaryAttackSingle;
            actions.FindActionMap("Player").FindAction("PrimaryAttackHold").performed += PrimaryAttackHold;
            actions.FindActionMap("Player").FindAction("StopAttack").performed += StopAttack;
            actions.FindActionMap("Player").FindAction("Reload").performed += Reload;
            actions.FindActionMap("Player").FindAction("Drop").performed += DropWeapon;
            _weaponScroll = actions.FindActionMap("Player").FindAction("WeaponScroll");
        }

        private void OnDestroy()
        {
            actions.FindActionMap("Player").FindAction("Interact").performed -= Interact;
            actions.FindActionMap("Player").FindAction("PrimaryAttackSingle").performed -= PrimaryAttackSingle;
            actions.FindActionMap("Player").FindAction("PrimaryAttackHold").performed -= PrimaryAttackHold;
            actions.FindActionMap("Player").FindAction("StopAttack").performed -= StopAttack;
            actions.FindActionMap("Player").FindAction("Reload").performed -= Reload;
            actions.FindActionMap("Player").FindAction("Drop").performed -= DropWeapon;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Raycast();
        }

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
            WeaponScroll();
        }

        private void WeaponScroll()
        {
            if (_weapons.Count >= 1)
            {
                var scroll = _weaponScroll.ReadValue<float>();
                if (scroll > 0)
                {
                    SelectWeapon((_currentWeaponIndex + 1) % _weapons.Count);
                }
                else if (scroll < 0)
                {
                    var index = (_currentWeaponIndex - 1) % _weapons.Count;
                    if (index < 0)
                    {
                        index = _weapons.Count - 1;
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
            weapon.Pickup(weaponHolder, this);
            AddWeapon(weapon);
        }

        private void PrimaryAttackSingle(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            _weapons[_currentWeaponIndex].AttackSingle();
        }

        private void PrimaryAttackHold(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            _weapons[_currentWeaponIndex].AttackHold();
        }

        private void StopAttack(InputAction.CallbackContext obj)
        {
            if (!HasWeapon()) return;
            _weapons[_currentWeaponIndex].StopAttack();
        }

        private void Reload(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            var weapon = (ShootingWeaponBase)_weapons[_currentWeaponIndex];
            weapon.Reload();
        }

        private void AddWeapon(WeaponBase weapon)
        {
            _weapons.Add(weapon);
            SelectWeapon(_weapons.Count - 1);
        }

        private void DropWeapon(InputAction.CallbackContext context)
        {
            if (!HasWeapon()) return;
            _weapons[_currentWeaponIndex].Drop();
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

        private bool HasWeapon()
        {
            if (_weapons.Count <= 0) return false;
            if (_weapons[_currentWeaponIndex] == null) return false;
            return true;
        }
    }
}