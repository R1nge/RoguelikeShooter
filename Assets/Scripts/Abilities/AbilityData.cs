using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability", order = 0)]
    public class AbilityData : ScriptableObject
    {
        [SerializeField] private int manaCost;
        [SerializeField] private float coolDown;
        [SerializeField] private string abilityName;
        [SerializeField] private Sprite sprite;

        public int GetManaCost() => manaCost;
        public float GetCoolDown() => coolDown;
        public string GetName() => abilityName;
        public Sprite GetSprite() => sprite;
    }
}