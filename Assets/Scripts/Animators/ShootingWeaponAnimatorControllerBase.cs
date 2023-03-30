using UnityEngine;

namespace Animators
{
    public abstract class ShootingWeaponAnimatorControllerBase : WeaponAnimatorControllerBase
    {
        private static readonly int ShootAnim = Animator.StringToHash("Shoot");
        private static readonly int ReloadAnim = Animator.StringToHash("Reload");

        public override void AttackPrimarySingle()
        {
            animator.SetBool(ShootAnim, true);
        }

        public override void AttackPrimaryHold()
        {
            animator.SetBool(ShootAnim, true);
        }

        public override void StopAttack()
        {
            animator.SetBool(ShootAnim, false);
        }

        public virtual void Reload()
        {
            animator.SetBool(ReloadAnim, true);
            animator.SetBool(ShootAnim, false);
        }

        public virtual void StopReload()
        {
            animator.SetBool(ReloadAnim, false);
        }
    }
}