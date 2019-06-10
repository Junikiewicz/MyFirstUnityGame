using UnityEngine;
using MyRPGGame.Statistic;

namespace MyRPGGame.Enemies
{
    public class Mage : Enemy
    {
        public GameObject fireball;
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
            theAn.SetTrigger("SpellCast");
            if(fireball)
            {
                GameObject missile = Instantiate(fireball, transform.position, Quaternion.identity);
                FireBall fireballComponent = missile.GetComponent<FireBall>();
                if(fireball)
                {
                    fireballComponent.SetDamage(stats.GetStat(typeof(AttackDamage)));
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find fireballComponent on prefab.");
                }
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find fireball prefab.");
            }
        }
    }
}

