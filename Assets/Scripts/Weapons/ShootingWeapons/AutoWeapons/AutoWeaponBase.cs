using Player;
using UnityEngine;

namespace Weapons.ShootingWeapons.AutoWeapons
{
    public class AutoWeaponBase : ShootingWeaponBase
    {
        public override void Pickup(Transform parent, PlayerWeaponController owner)
        {
            base.Pickup(parent, owner);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}