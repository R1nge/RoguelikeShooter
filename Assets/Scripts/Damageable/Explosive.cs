using Cinemachine;
using UnityEngine;

namespace Damageable
{
    public class Explosive : MonoBehaviour, IDamageable
    {
        [SerializeField] private int damage;
        [SerializeField] private float damageRadius;
        [SerializeField] private CinemachineImpulseSource impulse;
        [SerializeField] private AudioSource explosionSound;
        private readonly Collider[] _hits = new Collider[30];

        public void TakeDamage(int amount)
        {
            Explode();
        }

        private void Explode()
        {
            impulse.GenerateImpulse();
            Instantiate(explosionSound);
            Damage();
            Destroy(gameObject);
        }

        private void Damage()
        {
            var hits = Physics.OverlapSphereNonAlloc(transform.position, damageRadius, _hits);
            if (hits == 0) return;
            for (int i = 1; i < hits; i++)
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
        }
    }
}