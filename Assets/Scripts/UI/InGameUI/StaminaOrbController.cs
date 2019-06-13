using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class StaminaOrbController : MonoBehaviour
    {
        [SerializeField] private Image staminaOrb;
        [SerializeField] private Text staminaAmount;

        private float currentStamina;
        private float maximumStamina;
        private void Awake()
        {
            EventManager.Instance.AddListener<OnPlayerStaminaChanged>(UpdateStamina);
            EventManager.Instance.AddListener<OnPlayerMaxStaminaChanged>(UpdateMaximumStamina);
        }
        private void UpdateStamina(OnPlayerStaminaChanged eventData)
        {
            currentStamina = (float)eventData.CurrentStamina;
            UpdateOrb();
        }
        private void UpdateMaximumStamina(OnPlayerMaxStaminaChanged eventData)
        {
            maximumStamina = (float)eventData.MaximumStamina;
            UpdateOrb();
        }
        private void UpdateOrb()
        {
            staminaOrb.fillAmount = currentStamina / maximumStamina;
            staminaAmount.text = currentStamina.ToString();
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerStaminaChanged>(UpdateStamina);
            EventManager.Instance.RemoveListener<OnPlayerMaxStaminaChanged>(UpdateMaximumStamina);
        }
    }
}