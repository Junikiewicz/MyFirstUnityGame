using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class ExperimenceBarController : MonoBehaviour
    {
        public Image expBar;

        private double currentExperimence;
        private double requiredExperimence;
        private void Awake()
        {
            if (expBar)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerExperimenceChanged>(UpdateExperimence);
                    EventManager.Instance.AddListener<OnPlayerRequiredExperimenceChanged>(UpdateRequiredExperimence);
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
        private void UpdateExperimence(OnPlayerExperimenceChanged eventDate)
        {
            currentExperimence = eventDate.CurrentExperimence;
            UpdateBar();
        }
        private void UpdateRequiredExperimence(OnPlayerRequiredExperimenceChanged eventDate)
        {
            requiredExperimence = eventDate.RequiredExperimence;
            UpdateBar();
        }
        private void UpdateBar()
        {
            expBar.fillAmount = (float)(currentExperimence / requiredExperimence);
        }
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPlayerExperimenceChanged>(UpdateExperimence);
                EventManager.Instance.RemoveListener<OnPlayerRequiredExperimenceChanged>(UpdateRequiredExperimence);
            }
        }
    }
}