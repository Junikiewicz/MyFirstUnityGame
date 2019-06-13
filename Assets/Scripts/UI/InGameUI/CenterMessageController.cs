using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class CenterMessageController : MonoBehaviour
    {
        [SerializeField] private Text message;

        private Animator animator;
        private const string OnDeathMessage = "YOU DIED";
        private const string OnLevelCompletedMessage = "LEVEL COMPLETED";
        private const string YouDiedTrigger = "YouDied";
        private const string LevelCompletedTrigger = "LevelCompleted";
        private void Awake()
        {
            animator = GetComponent<Animator>();

            EventManager.Instance.AddListener<OnPlayerKilled>(ShowYouDiedMessage);
            EventManager.Instance.AddListener<OnLevelCompleted>(ShowLevelCompletedMessage);
        }
        private void ShowYouDiedMessage(OnPlayerKilled eventData)
        {
            message.text = OnDeathMessage;
            animator.SetTrigger(YouDiedTrigger);
        }
        private void ShowLevelCompletedMessage(OnLevelCompleted eventData)
        {
            message.text = OnLevelCompletedMessage;
            animator.SetTrigger(LevelCompletedTrigger);
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerKilled>(ShowYouDiedMessage);
            EventManager.Instance.RemoveListener<OnLevelCompleted>(ShowLevelCompletedMessage);
        }
    }
}
