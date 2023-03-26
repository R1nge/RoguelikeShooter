using TMPro;
using UnityEngine;

namespace UI
{
    public class ManaUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI manaText;

        public void UpdateUI(int value)
        {
            manaText.SetText($"Mana: {value}");
        }
    }
}