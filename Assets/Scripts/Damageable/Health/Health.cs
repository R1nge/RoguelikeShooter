using System;
using UnityEngine;

namespace Damageable
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] protected int maxHealth;
        protected int CurrentHealth;

        public event Action<int> InitEvent;
        public event Action<int> OnDamagedEvent;
        public event Action<int> OnHealedEvent;
        public event Action OnDeathEvent;

        public void Heal(int amount)
        {
            if (CurrentHealth + amount > maxHealth)
            {
                CurrentHealth = maxHealth;
            }
            else
            {
                CurrentHealth += amount;
            }

            OnHealedEvent?.Invoke(CurrentHealth);
        }

        private void Awake() => CurrentHealth = maxHealth;

        private void Start() => InitEvent?.Invoke(CurrentHealth);

        public void TakeDamage(int amount)
        {
            DamageCallback(amount);
        }

        protected virtual void DamageCallback(int amount)
        {
            CurrentHealth -= amount;

            if (CurrentHealth > 0)
            {
                OnDamagedEvent?.Invoke(CurrentHealth);
            }
            else
            {
                OnDeathEvent?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}