using System;
using TMPro;
using UnityEngine;
using Weapons;
using Weapons.ShootingWeapons;

namespace UI
{
    public class WeaponPickupUI : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            var cameraTransform = playerCamera.transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance, layerMask))
            {
                if (hit.transform.TryGetComponent(out WeaponBase weapon))
                {
                    if (weapon.CanPickupWeapon())
                    {
                        var weaponInfo = weapon.GetWeaponInfo();
                        text.text = $"Pickup: {weaponInfo.weaponName}";
                        text.color = weaponInfo.rarenessColors.colors[(int) weaponInfo.rareness];
                    }
                    else
                    {
                        text.text = string.Empty;
                    }
                }
                else
                {
                    text.text = string.Empty;
                }
            }
            else
            {
                text.text = string.Empty;
            }
        }
    }
}