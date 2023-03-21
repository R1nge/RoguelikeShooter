using UnityEngine;

namespace Damageable
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private Component[] childDamageables;
        
        public void TakeDamage(int amount)
        {
            for (int i = 0; i < childDamageables.Length; i++)
            {
                if (childDamageables[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(amount);
                }
            }
            
            Destroy(gameObject);
        }
    }
}