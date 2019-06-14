using MyRPGGame.Player;
using MyRPGGame.Statistics;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class BowMan : Enemy
    {
        [SerializeField] private GameObject arrow;

        private const string AnimatorShotTrigger = "Shot";
        protected override void Awake()
        {
            base.Awake();
            characterClass = EnemyClass.bowman;
        }
        protected override void Attack()
        {
            if (PlayerController.Instance != null)
            {
                theAn.SetTrigger(AnimatorShotTrigger);
                Invoke(nameof(LauchArrow), 1f);
            }
        }
        void LauchArrow()
        {
            if (!pause)
            {
                GameObject missile = Instantiate(arrow, transform.position, Quaternion.identity);
                Arrow arrowComponent = missile.GetComponent<Arrow>();
                arrowComponent.SetDamage(stats.GetStat(Stat.AttackDamage));
            }
        }
    }
}