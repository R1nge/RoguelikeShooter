using UnityEngine;

namespace Damageable
{
    public class Explosive : MonoBehaviour, IDamageable
    {
        [SerializeField] private float damage;
        [SerializeField] private float range;
        
        public void TakeDamage(int amount)
        {
            Explode();
        }

        private void Explode()
        {
            print("Explosive");
        }
    }
}