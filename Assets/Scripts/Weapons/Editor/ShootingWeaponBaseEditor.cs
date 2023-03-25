using UnityEditor;
using Weapons.ShootingWeapons;

namespace Weapons.Editor
{
    [CustomEditor(typeof(ShootingWeaponBase), true)]
    [CanEditMultipleObjects]
    public class ShootingWeaponBaseEditor : UnityEditor.Editor
    {
        private static SerializedProperty _maxAmmo;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShootingWeaponBase weapon = (ShootingWeaponBase)target;
            
            serializedObject.Update();

            _maxAmmo = serializedObject.FindProperty("_maxAmmoAmount");

            if (!weapon.IsInfinite())
            {
                _maxAmmo.intValue = EditorGUILayout.IntField("Max ammo amount: ", _maxAmmo.intValue);
                weapon.SetMaxAmmoAmount(_maxAmmo.intValue);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}