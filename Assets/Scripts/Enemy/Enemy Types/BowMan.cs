using MyRPGGame.Player;
using MyRPGGame.Statistic;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class BowMan : Enemy
    {
        public GameObject arrow;
        protected override void Awake()
        {
            base.Awake();
            characterClass = EnemyClass.bowman;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Attack()
        {
            base.Attack();
            if (PlayerController.Instance != null)
            {
                theAn.SetTrigger("Shot");
                Invoke(nameof(LauchArrow), 1f);
            }
        }
        void LauchArrow()
        {
            if(!pause)
            {
                if (arrow)
                {
                    GameObject missile = Instantiate(arrow, transform.position, Quaternion.identity);
                    Arrow arrowComponent = missile.GetComponent<Arrow>();
                    if (arrowComponent)
                    {
                        arrowComponent.SetDamage(stats.GetStat(typeof(AttackDamage)));
                    }
                    else
                    {
                        Debug.LogError(GetType() + " couldn't find arrowComponent on prefab.");
                    }
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find arrow prefab.");
                }
            } 
        }
    }
}