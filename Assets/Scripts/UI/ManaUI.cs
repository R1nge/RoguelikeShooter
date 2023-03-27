using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ManaUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI manaText;
        private readonly Dictionary<int, string> _numbersDictionary = new(101);

        private void Awake()
        {
            for (int i = 0; i < 101; i++)
            {
                _numbersDictionary.Add(i, $"Mana: {i}");
            }
        }
        
        public void UpdateUI(int value)
        {
            if (_numbersDictionary.TryGetValue(value, out var str))
            {
                manaText.SetText(str);
            }
        }
    }
}