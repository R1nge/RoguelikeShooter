using UnityEngine;

namespace Animators
{
    public abstract class WeaponAnimatorControllerBase : MonoBehaviour
    {
        [SerializeField] protected Animator animator;

        public abstract void AttackSingle();

        public abstract void AttackHold();

        public abstract void StopAttack();
    }
}