using Damageable;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.InitEvent += UpdateUI;
            _health.OnDamagedEvent += UpdateUI;
        }

        private void UpdateUI(int health) => healthText.text = $"Health: {health}";

        private void OnDestroy()
        {
            _health.InitEvent -= UpdateUI;
            _health.OnDamagedEvent -= UpdateUI;
        }
    }
}