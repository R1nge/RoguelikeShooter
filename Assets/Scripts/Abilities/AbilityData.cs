using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public int manaCost;
        public float coolDown;
        public string abilityName;
        public Sprite sprite;
    }
}