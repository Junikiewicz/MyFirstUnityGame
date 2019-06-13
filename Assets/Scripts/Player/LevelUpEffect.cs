using MyRPGGame.Events;
using UnityEngine;

namespace MyRPGGame.Player
{
    public class LevelUpEffect : MonoBehaviour
    {
        private AudioSource audioSource;
        private Animator animator;
        private const string LvlUpTrigger = "LvlUp";
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            EventManager.Instance.AddListener<OnPlayerLvlChanged>(Animate);
        }
        private void Animate(OnPlayerLvlChanged data)
        {
                audioSource.Play();
                animator.SetTrigger(LvlUpTrigger);
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerLvlChanged>(Animate);
        }
    }
}
