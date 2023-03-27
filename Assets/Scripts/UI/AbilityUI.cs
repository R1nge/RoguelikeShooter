using Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private Image abilityImage;
        private float _imageFill;

        private void Awake() => abilityImage.enabled = abilityImage.sprite != null;

        public void UpdateUI(AbilityData abilityData)
        {
            if (abilityData == null)
            {
                abilityImage.enabled = false;
            }
            else
            {
                abilityImage.sprite = abilityData.GetSprite();
                abilityImage.type = Image.Type.Filled;
                abilityImage.enabled = true;
            }
        }

        public void UpdateFill(AbilityData abilityData, float coolDownLeft)
        {
            abilityImage.fillAmount = coolDownLeft / abilityData.GetCoolDown();
        }
    }
}