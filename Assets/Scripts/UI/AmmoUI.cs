using TMPro;
using UnityEngine;

namespace UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;

        public void UpdateUI(int ammoAmount, int maxAmmo, bool isInfinite)
        {
            ammoText.text = isInfinite ? $"{ammoAmount} / ∞" : $"{ammoAmount} / {maxAmmo}";
        }
    }
}