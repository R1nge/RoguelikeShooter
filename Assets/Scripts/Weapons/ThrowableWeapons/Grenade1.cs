using Cinemachine;
using Damageable;
using Player;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Grenade1 : WeaponBase
    {
        [SerializeField] protected float throwForce;
        [SerializeField] protected int damage;
        [SerializeField] protected float damageRadius;
        [SerializeField] protected float timeBeforeExplosion;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected CinemachineImpulseSource impulse;
        [SerializeField] protected AudioSource explosionSound;
        private readonly Collider[] _hits = new Collider[30];

        public override void AttackSingle()
        {
            Rigidbody.isKinematic = false;
            transform.parent = null;
            Collider.isTrigger = false;
            Throw();
            RemoveFromInventory();
            Invoke(nameof(Explode), timeBeforeExplosion);
        }

        public override void Pickup(Transform parent, PlayerWeaponController owner)
        {
            base.Pickup(parent, owner);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void Throw()
        {
            Rigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        private void Explode()
        {
            impulse.GenerateImpulse();
            Instantiate(explosionSound);
            Damage();
        }

        private void Damage()
        {
            var hits = Physics.OverlapSphereNonAlloc(transform.position, damageRadius, _hits, layerMask);
            if (hits == 0) return;
            for (int i = 0; i < hits; i++)
            {
                var hitTransform = _hits[i].transform;
                if (Physics.Raycast(transform.position, hitTransform.position, out var hit, damageRadius))
                {
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(damage);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}