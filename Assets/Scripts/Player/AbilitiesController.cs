using System.Collections;
using Abilities;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AbilitiesController : MonoBehaviour
    {
        [SerializeField, Tooltip("Per second")]
        private float manaRestoreRate;

        [SerializeField] private int maxManaAmount;
        [SerializeField] private InputActionAsset actions;
        private int _currentManaAmount;
        private AbilityBase _currentAbility;
        private IEnumerator _abilityCoroutine;
        private IEnumerator _manaRestore;
        private ManaUI _manaUI;
        private AbilityUI _abilityUI;

        public void SetAbility(AbilityBase ability)
        {
            _currentAbility = ability;
            _abilityUI.UpdateUI(ability.GetAbilityData());
            _abilityUI.UpdateFill(ability.GetAbilityData(), ability.GetAbilityData().GetCoolDown());
        }

        private void Awake()
        {
            _manaUI = GetComponent<ManaUI>();
            _abilityUI = GetComponent<AbilityUI>();
            _currentManaAmount = maxManaAmount;
        }

        private void Start()
        {
            _manaUI.UpdateUI(_currentManaAmount);
            StartCoroutine(RestoreMana());
        }

        private void Update()
        {
            if (actions.FindActionMap("Player").FindAction("SpecialAbility").WasPressedThisFrame())
            {
                StartCoroutine(UseAbility_c());
            }
        }

        private IEnumerator UseAbility_c()
        {
            if (_currentAbility == null)
            {
                print("No active ability");
                yield break;
            }

            if (_abilityCoroutine != null)
            {
                print("Waiting for cooldown");
                yield break;
            }

            var manaCost = _currentAbility.GetAbilityData().GetManaCost();
            if (_currentManaAmount - manaCost >= 0)
            {
                print($"{_currentAbility}");
                _currentManaAmount -= manaCost;
                _abilityCoroutine = _currentAbility?.Execute();
                _manaUI.UpdateUI(_currentManaAmount);
                _abilityUI.UpdateUI(_currentAbility?.GetAbilityData());
                yield return StartCoroutine(_abilityCoroutine);
                _abilityCoroutine = null;
            }
            else
            {
                print("Not enough mana");
            }
        }

        //TODO: use update()???
        private IEnumerator RestoreMana()
        {
            while (enabled)
            {
                if (_currentManaAmount >= maxManaAmount)
                {
                    yield return new WaitForSeconds(1 / manaRestoreRate);
                }
                else
                {
                    yield return new WaitForSeconds(1 / manaRestoreRate);
                    _currentManaAmount += 1;
                    _manaUI.UpdateUI(_currentManaAmount);
                }
            }
        }
    }
}