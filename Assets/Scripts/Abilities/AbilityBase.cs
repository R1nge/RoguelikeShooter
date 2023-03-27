using System.Collections;

namespace Abilities
{
    public abstract class AbilityBase
    {
        private readonly AbilityData _abilityData;

        protected AbilityBase(AbilityData abilityData)
        {
            _abilityData = abilityData;
        }

        public abstract IEnumerator Execute();

        public AbilityData GetAbilityData() => _abilityData;
    }
}