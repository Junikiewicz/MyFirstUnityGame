using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class BloodOverlayController : MonoBehaviour
    {
        public Image bloodOverlay;

        private double currentHealth;
        private double maximumHealth;
        private void Awake()
        {
            if (bloodOverlay)
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
        private void UpdateHealth(OnPlayerHealthChanged eventData)
        {
            currentHealth = eventData.CurrentHealth;
            UpdateOverlayEffect();
        }
        private void UpdateMaximumHealth(OnPlayerMaxHealthChanged eventDate)
        {
            maximumHealth = eventDate.MaxiumHealth;
            UpdateOverlayEffect();
        }
        private void UpdateOverlayEffect()
        {
            var tempColor = bloodOverlay.color;
            tempColor.a = -2 * (float)(currentHealth / maximumHealth) + 1; //simple linear function when x=0 then y=1 and when x = 0.5 then y = 0 
            bloodOverlay.color = tempColor;
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