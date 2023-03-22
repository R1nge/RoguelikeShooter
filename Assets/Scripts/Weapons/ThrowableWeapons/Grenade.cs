using Cinemachine;
using Damageable;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class Grenade : ThrowableWeaponBase
    {
        [SerializeField] protected float damageRadius;
        [SerializeField] protected float timeBeforeExplosion;
        [SerializeField] protected CinemachineImpulseSource impulse;
        [SerializeField] protected AudioSource explosionSound;
        private readonly RaycastHit[] _hits = new RaycastHit[30];
        private bool _canPickup = true;

        public override void Attack()
        {
            Drop();
            Throw();
            _canPickup = false;
            Invoke(nameof(Explode), timeBeforeExplosion);
        }

        protected virtual void Throw()
        {
            //TODO: use projectile motion
        }

        protected virtual void Explode()
        {
            impulse.GenerateImpulse();
            Instantiate(explosionSound);
            Damage();
        }

        private void Damage()
        {
            var hits = Physics.SphereCastNonAlloc(transform.position, damageRadius, transform.forward, _hits);
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

        public override void Pickup(Transform parent)
        {
            if (!_canPickup) return;
            base.Pickup(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        protected override void OnCollisionEnter(Collision other)
        {
        }

        protected override void OnTriggerEnter(Collider other)
        {
        }
    }
}