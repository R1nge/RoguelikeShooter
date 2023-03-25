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
        [SerializeField] protected GameObject explosionVfx;
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

        private void Throw()
        {
            Rigidbody.AddForce(CalculateVelocity(throwForce, transform.forward, 45f), ForceMode.Impulse);
        }

        private Vector3 CalculateVelocity(float speed, Vector3 direction, float degreeAngle)
        {
            //Raycast as distance
            //float radians = (rayLength / maxDistance) * 0.01745329252f;
            float radians = degreeAngle * 0.01745329252f;
            
            float yDirection = Mathf.Tan(radians);

            Vector3 finalDirection = new Vector3(direction.x, yDirection, direction.z);

            return speed * finalDirection.normalized;
        }

        private void Explode()
        {
            impulse.GenerateImpulse();
            Instantiate(explosionSound, transform.position, Quaternion.identity);
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
            Damage();
            Destroy(gameObject);
        }

        private void Damage()
        {
            var hits = Physics.OverlapSphereNonAlloc(transform.position, damageRadius, _hits, layerMask);
            if (hits == 0) return;
            for (int i = 0; i < hits; i++)
            {
                var hitTransform = _hits[i].transform;
                if (Physics.Raycast(transform.position, hitTransform.position - transform.position, out var hit,
                        damageRadius))
                {
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    }
}