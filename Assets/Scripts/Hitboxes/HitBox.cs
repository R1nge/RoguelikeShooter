using Damageable;
using UnityEngine;

namespace Hitboxes
{
    public class HitBox : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float damageModifier;
        private Health _health;

        private void Awake() => _health = GetComponentInParent<Health>();

        public void TakeDamage(int amount) => ApplyModifier(amount);

        private void ApplyModifier(int damage) => _health.TakeDamage(Mathf.RoundToInt(damage * damageModifier));
    }
}