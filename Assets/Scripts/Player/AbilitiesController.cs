using System;
using System.Collections;
using Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AbilitiesController : MonoBehaviour
    {
        [SerializeField] private int maxManaAmount;
        [SerializeField] private InputActionAsset actions;
        private int _currentManaAmount;
        private AbilityBase _currentAbility;
        private IEnumerator _abilityCoroutine;

        private void Awake()
        {
            _currentManaAmount = maxManaAmount;
        }

        private void Update()
        {
            if (actions.FindActionMap("Player").FindAction("SpecialAbility").WasPressedThisFrame())
            {
                StartCoroutine(UseAbility());
            }
        }

        public void SetAbility(AbilityBase ability)
        {
            _currentAbility = ability;
        }

        private IEnumerator UseAbility()
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

            var manaCost = _currentAbility.GetAbilityCost();
            if (_currentManaAmount - manaCost >= 0)
            {
                print($"{_currentAbility}");
                _currentManaAmount -= manaCost;
                _abilityCoroutine = _currentAbility?.Execute();
                yield return StartCoroutine(_abilityCoroutine);
                _abilityCoroutine = null;
            }
            else
            {
                print("Not enough mana");
            }
        }
    }
}