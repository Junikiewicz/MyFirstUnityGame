using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class InfoPanelController : MonoBehaviour
    {
        public Text playerLvl;
        public Text goldAmount;
        private void Awake()
        {
            if (playerLvl&&goldAmount)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerLvlChanged>(UpdateLvl);
                    EventManager.Instance.AddListener<OnPlayerGoldChanged>(UpdateGold);
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
        private void UpdateLvl(OnPlayerLvlChanged eventDate)
        {

            playerLvl.text = eventDate.CurrentLvl.ToString();
        }

        private void UpdateGold(OnPlayerGoldChanged eventData)
        {
            goldAmount.text = eventData.CurrentGold.ToString();
        }

        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerLvlChanged>(UpdateLvl);
                EventManager.Instance.RemoveListener<OnPlayerGoldChanged>(UpdateGold);
            }
        }
    }

}
