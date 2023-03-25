using UnityEditor;
using Weapons.ShootingWeapons;

namespace Weapons.Editor
{
    [CustomEditor(typeof(ShootingWeaponBase), true)]
    [CanEditMultipleObjects]
    public class ShootingWeaponBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShootingWeaponBase weapon = (ShootingWeaponBase)target;

            if (!weapon.IsInfinite())
            {
                var amount = EditorGUILayout.IntField("Max ammo amount: ", weapon.GetMaxAmmoAmount());
                weapon.SetMaxAmmoAmount(amount);
            }
        }
    }
}