using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Kunai : ThrowableWeaponBase
    {
        private bool _canBounce;

        public override void AttackSingle()
        {
            Drop();
            Rigidbody.AddForce(-transform.forward * force, ForceMode.Impulse);
            _canBounce = true;
        }

        public override void Pickup(Transform parent)
        {
            base.Pickup(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            if (_canBounce)
            {
                var dir = Vector3.Reflect(other.contacts[0].point - transform.position, other.contacts[0].normal);
                Rigidbody.AddForce(dir * force, ForceMode.Impulse);
                _canBounce = false;
            }
        }
    }
}