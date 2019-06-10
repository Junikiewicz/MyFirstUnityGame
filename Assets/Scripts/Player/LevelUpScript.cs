using MyRPGGame.Events;
using UnityEngine;

namespace MyRPGGame.Player
{
    public class LevelUpScript : MonoBehaviour
    {
        public AudioSource audioEffect;
        public Animator animator;
        private void Start()
        {
            EventManager.Instance.AddListener<OnPlayerLvlChanged>(Animate);
        }
        private void Animate(OnPlayerLvlChanged data)
        {
                audioEffect.Play();
                animator.SetTrigger("LvlUp");
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerLvlChanged>(Animate);
        }
    }
}
