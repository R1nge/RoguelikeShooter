using UnityEngine;

namespace Weapons.ShootingWeapons.AutoWeapons
{
    public class BounceRifle : ShootingWeaponBase
    {
        public override void AttackSingle()
        {
        }

        public override void Pickup(Transform parent)
        {
            base.Pickup(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}