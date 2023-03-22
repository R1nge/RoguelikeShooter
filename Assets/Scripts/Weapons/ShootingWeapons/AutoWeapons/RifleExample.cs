using UnityEngine;

namespace Weapons.ShootingWeapons.AutoWeapons
{
    public class RifleExample : ShootingWeaponBase
    {
        public override void Pickup(Transform parent)
        {
            base.Pickup(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}