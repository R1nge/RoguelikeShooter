using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Kunai : ThrowableWeaponBase
    {
        public override void Pickup(Transform parent)
        {
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 180, 0));
        }
    }
}