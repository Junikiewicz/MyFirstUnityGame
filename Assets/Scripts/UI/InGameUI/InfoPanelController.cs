using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class InfoPanelController : MonoBehaviour
    {
        [SerializeField] private Text playerLvl;
        [SerializeField] private Text goldAmount;
        private void Awake()
        {
            EventManager.Instance.AddListener<OnPlayerLvlChanged>(UpdateLvl);
            EventManager.Instance.AddListener<OnPlayerGoldChanged>(UpdateGold);
        }
        private void UpdateLvl(OnPlayerLvlChanged eventData)
        {
            playerLvl.text = eventData.CurrentLvl.ToString();
        }
        private void UpdateGold(OnPlayerGoldChanged eventData)
        {
            goldAmount.text = eventData.CurrentGold.ToString();
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerLvlChanged>(UpdateLvl);
            EventManager.Instance.RemoveListener<OnPlayerGoldChanged>(UpdateGold);
        }
    }

}
