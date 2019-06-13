using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class NumberOfEnemiesController : MonoBehaviour
    {
        [SerializeField] private Text numberOfEnemies;
        private void Awake()
        {
            EventManager.Instance.AddListener<OnNumberOfEnemiesChanged>(UpdateNumberOfEnemies);
        }
        private void UpdateNumberOfEnemies(OnNumberOfEnemiesChanged eventData)
        {
            numberOfEnemies.text = eventData.numberOfEnemies.ToString();
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnNumberOfEnemiesChanged>(UpdateNumberOfEnemies);
        }
    }
}
