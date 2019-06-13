using UnityEngine;
using UnityEngine.UI;
using MyRPGGame.Events;

namespace MyRPGGame.UI
{
    public class ExperimenceBarController : MonoBehaviour
    {
        [SerializeField] private Image expBar;

        private double currentExperimence;
        private double requiredExperimence;
        private void Awake()
        {
            EventManager.Instance.AddListener<OnPlayerExperimenceChanged>(UpdateExperimence);
            EventManager.Instance.AddListener<OnPlayerRequiredExperimenceChanged>(UpdateRequiredExperimence);
        }
        private void UpdateExperimence(OnPlayerExperimenceChanged eventData)
        {
            currentExperimence = eventData.CurrentExperimence;
            UpdateBar();
        }
        private void UpdateRequiredExperimence(OnPlayerRequiredExperimenceChanged eventData)
        {
            requiredExperimence = eventData.RequiredExperimence;
            UpdateBar();
        }
        private void UpdateBar()
        {
            expBar.fillAmount = (float)(currentExperimence / requiredExperimence);
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerExperimenceChanged>(UpdateExperimence);
            EventManager.Instance.RemoveListener<OnPlayerRequiredExperimenceChanged>(UpdateRequiredExperimence);
        }
    }
}