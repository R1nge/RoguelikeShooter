using System;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Weapon Info", menuName = "Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        public string weaponName;
        public Rareness rareness;
        public RarenessColors rarenessColors;

        private void OnValidate()
        {
            Color[] _colors = rarenessColors.colors;
            var len = Enum.GetNames(typeof(Rareness)).Length;
            rarenessColors.colors = new Color[len];
            for (int i = 0; i < len; i++)
            {
                rarenessColors.colors[i] = _colors[i];
            }
        }
    }
}