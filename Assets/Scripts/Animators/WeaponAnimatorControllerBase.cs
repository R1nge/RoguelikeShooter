using UnityEngine;

namespace Animators
{
    public abstract class WeaponAnimatorControllerBase : MonoBehaviour
    {
        [SerializeField] protected Animator animator;

        public abstract void AttackPrimarySingle();

        public abstract void AttackPrimaryHold();

        public abstract void StopAttack();
    }
}