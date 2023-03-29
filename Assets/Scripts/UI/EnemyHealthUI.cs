using Damageable;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.InitEvent += UpdateUI;
            _health.OnDamagedEvent += UpdateUI;
            _health.OnHealedEvent += UpdateUI;
        }

        private void UpdateUI(int health) => healthText.text = health.ToString();

        private void OnDestroy()
        {
            _health.InitEvent -= UpdateUI;
            _health.OnDamagedEvent -= UpdateUI;
            _health.OnHealedEvent -= UpdateUI;
        }
    }
}