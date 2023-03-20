namespace Weapons.ShootingWeapons.AutoWeapons
{
    public class AutoWeapon : ShootingWeaponBase
    {
        public override void Attack()
        {
            print("auto weapon: attack");
        }

        public override void Reload()
        {
            print("auto weapon: reload");
        }
    }
}