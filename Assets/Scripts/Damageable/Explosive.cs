using Cinemachine;
using UnityEngine;

namespace Damageable
{
    public class Explosive : MonoBehaviour, IDamageable
    {
        [SerializeField] private int damage;
        [SerializeField] private float damageRadius;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private CinemachineImpulseSource impulse;
        [SerializeField] private AudioSource explosionSound;
        [SerializeField] protected GameObject explosionVfx;
        private readonly Collider[] _hits = new Collider[30];
        private bool _exploded;

        public void TakeDamage(int amount)
        {
            if (_exploded) return;
            Explode();
        }

        private void Explode()
        {
            _exploded = true;
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
            for (int i = 1; i < hits; i++)
            {
                var hitTransform = _hits[i].transform;
                if (Physics.Raycast(transform.position, hitTransform.position - transform.position, out var hit, damageRadius))
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