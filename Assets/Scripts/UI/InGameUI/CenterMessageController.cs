using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class CenterMessageController : MonoBehaviour
    {
        public Text message;
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator)
            {
                if (message)
                {
                    if (EventManager.Instance)
                    {
                        EventManager.Instance.AddListener<OnPlayerKilled>(ShowYouDiedMessage);
                        EventManager.Instance.AddListener<OnLevelCompleted>(ShowLevelCompletedMessage);
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
            else
            {
                Debug.LogError(GetType() + " couldn't find animator");
            }
        }
        private void ShowYouDiedMessage(OnPlayerKilled data)
        {
            message.text = "YOU DIED";
            animator.SetTrigger("YouDied");
        }
        private void ShowLevelCompletedMessage(OnLevelCompleted data)
        {
            message.text = "LEVEL COMPLETED";
            animator.SetTrigger("LevelCompleted");
        }
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerKilled>(ShowYouDiedMessage);
                EventManager.Instance.RemoveListener<OnLevelCompleted>(ShowLevelCompletedMessage);
            }
        }
    }
}
