using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class NumberOfEnemiesController : MonoBehaviour
    {
        public Text numberOfEnemies;
        private void Awake()
        {
            if (numberOfEnemies)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnNumberOfEnemiesChanged>(UpdateNumberOfEnemies);
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
        private void UpdateNumberOfEnemies(OnNumberOfEnemiesChanged eventDate)
        {
            numberOfEnemies.text = eventDate.numberOfEnemies.ToString();
        }
        private void OnDestroy()
        {

            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnNumberOfEnemiesChanged>(UpdateNumberOfEnemies);
            }
        }
    }
}
