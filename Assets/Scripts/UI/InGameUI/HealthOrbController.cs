using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class HealthOrbController : MonoBehaviour
    {
        public Image healthOrb;
        public Text healthAmount;

        private double currentHealth;
        private double maximumHealth;

        private void Awake()
        {
            if (healthOrb && healthAmount)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerHealthChanged>(UpdateHealth);
                    EventManager.Instance.AddListener<OnPlayerMaxHealthChanged>(UpdateMaximumHealth);
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find EventManager.");
                }
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find one of its graphics components");
            }
        }
        public void UpdateHealth(OnPlayerHealthChanged eventData)
        {
            currentHealth = eventData.CurrentHealth;
            UpdateOrb();
        }
        private void UpdateMaximumHealth(OnPlayerMaxHealthChanged eventDate)
        {
            maximumHealth = eventDate.MaxiumHealth;
            UpdateOrb();
        }
        private void UpdateOrb()
        {
            healthOrb.fillAmount = (float)(currentHealth / maximumHealth);
            healthAmount.text = currentHealth.ToString();
        }
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerHealthChanged>(UpdateHealth);
                EventManager.Instance.RemoveListener<OnPlayerMaxHealthChanged>(UpdateMaximumHealth);
            }
        }
    }
}