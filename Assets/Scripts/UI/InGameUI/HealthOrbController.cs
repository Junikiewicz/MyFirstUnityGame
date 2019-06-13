using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class HealthOrbController : MonoBehaviour
    {
        [SerializeField] private Image healthOrb;
        [SerializeField] private Text healthAmount;

        private double currentHealth;
        private double maximumHealth;
        private void Awake()
        {
            EventManager.Instance.AddListener<OnPlayerHealthChanged>(UpdateHealth);
            EventManager.Instance.AddListener<OnPlayerMaxHealthChanged>(UpdateMaximumHealth);
        }
        public void UpdateHealth(OnPlayerHealthChanged eventData)
        {
            currentHealth = eventData.CurrentHealth;
            UpdateOrb();
        }
        private void UpdateMaximumHealth(OnPlayerMaxHealthChanged eventData)
        {
            maximumHealth = eventData.MaxiumHealth;
            UpdateOrb();
        }
        private void UpdateOrb()
        {
            healthOrb.fillAmount = (float)(currentHealth / maximumHealth);
            healthAmount.text = currentHealth.ToString();
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerHealthChanged>(UpdateHealth);
            EventManager.Instance.RemoveListener<OnPlayerMaxHealthChanged>(UpdateMaximumHealth);
        }
    }
}