using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class InfoPanelController : MonoBehaviour
    {
        public Text playerLvl;
        private void Awake()
        {
            if (playerLvl)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerLvlChanged>(UpdateLvl);
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
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerLvlChanged>(UpdateLvl);
            }
        }
    }

}
