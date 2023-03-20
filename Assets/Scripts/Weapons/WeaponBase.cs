using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected int damage;
        protected Rigidbody Rigidbody;

        protected virtual void Awake() => Rigidbody = GetComponent<Rigidbody>();

        public abstract void Attack();

        public virtual void Pickup(Transform parent)
        {
            Rigidbody.isKinematic = true;
            transform.parent = parent;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void Drop()
        {
            transform.parent = null;
            Rigidbody.isKinematic = false;
        }
    }
}