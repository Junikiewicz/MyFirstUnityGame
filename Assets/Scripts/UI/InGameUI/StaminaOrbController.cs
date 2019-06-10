using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class StaminaOrbController : MonoBehaviour
    {
        public Image staminaOrb;
        public Text staminaAmount;

        float currentStamina;
        float maximumStamina;


        private void Awake()
        {
            if (staminaAmount && staminaOrb)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerStaminaChanged>(UpdateStamina);
                    EventManager.Instance.AddListener<OnPlayerMaxStaminaChanged>(UpdateMaximumStamina);
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
        private void UpdateStamina(OnPlayerStaminaChanged test)
        {
            currentStamina = (float)test.CurrentStamina;
            UpdateOrb();
        }

        private void UpdateMaximumStamina(OnPlayerMaxStaminaChanged test)
        {
            maximumStamina = (float)test.MaximumStamina;
            UpdateOrb();
        }

        private void UpdateOrb()
        {
            staminaOrb.fillAmount = currentStamina / maximumStamina;
            staminaAmount.text = currentStamina.ToString();
        }
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerStaminaChanged>(UpdateStamina);
                EventManager.Instance.RemoveListener<OnPlayerMaxStaminaChanged>(UpdateMaximumStamina);
            }
        }
    }
}