using UnityEngine;
using MyRPGGame.Statistics;

namespace MyRPGGame.Enemies
{
    public class Mage : Enemy
    {
        [SerializeField] private GameObject fireball;
        private const string AnimatorSpellcastTrigger = "SpellCast";
        protected override void Awake()
        {
            base.Awake();
            characterClass = EnemyClass.mage;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Attack()
        {
            base.Attack();
            theAn.SetTrigger(AnimatorSpellcastTrigger);
            GameObject missile = Instantiate(fireball, transform.position, Quaternion.identity);
            FireBall fireballComponent = missile.GetComponent<FireBall>();
            fireballComponent.SetDamage(stats.GetStat(Stat.AttackDamage));
        }
    }
}

