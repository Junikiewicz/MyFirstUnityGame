using UnityEngine;
using MyRPGGame.Statistic;
using MyRPGGame.PathFinding;
using MyRPGGame.Player;
using MyRPGGame.Events;
using MyRPGGame.Templates;

namespace MyRPGGame.Enemies
{
    public class Enemy : MonoBehaviour, IDamageDealer
    {
        public static int numberOfEnemies = 0;
        public EnemyStatsTemplate enemyStatsTempalte;
        public EnemyClass characterClass;

        public GameObject damageTakenPopup;

        protected StatBlock stats;
        protected Animator theAn;

        private AudioSource audioSource;
        private Rigidbody2D enemyRigidbody;
        private Unit pathfindingUnit;
        private Vector3 currentPathfiningDirection;
        private double distanceFromPlayer;
        private bool moving = false;
        private float attackTimer = 0;
        private bool alive = true;
        protected bool pause = false;
        public Vector3 GetCurrentEnemyPosition()//hotfix due to wrongly placed pivot in 500+ sprites. Will be fixed as soon as i get sorting groups right.
        {
            return new Vector3(transform.position.x, transform.position.y - 1f, 0);
        }
        public double GetStat(System.Type stat)
        {
            return stats.GetStat(stat);
        }

        //used by player
        public double DealDamage()
        {
            return stats.GetStat(typeof(AttackDamage));
        }

        protected virtual void Awake()
        {
            enemyRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            stats = gameObject.AddComponent<StatBlock>();
            theAn = GetComponent<Animator>();
            pathfindingUnit = GetComponent<Unit>();
            if (enemyRigidbody && audioSource && stats && theAn && pathfindingUnit)
            {
                if (PlayerController.Instance)
                {
                    pathfindingUnit.Inicialize(PlayerController.Instance.GetCurrentPlayerPosition, GetCurrentEnemyPosition);
                }
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPauseStart>(StartPause);
                    EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
                    numberOfEnemies++;
                    EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(numberOfEnemies));
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find EventManager.");
                    enabled = false;
                }
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find one of its required components");
                enabled = false;
            }
        }
        protected virtual void Start()
        {
            if (GameplayController.Instance)
            {
                int lvl = GameplayController.Instance.currentWorldLevel;
                stats.AddStat(new AttackDamage(enemyStatsTempalte.startingAttackDamage + lvl * enemyStatsTempalte.attackDamageAddedOnPromotion));
                stats.AddStat(new AttackSpeed(enemyStatsTempalte.startingAttackSpeed + lvl * enemyStatsTempalte.attackSpeedAddedOnPromotion));
                stats.AddStat(new Health(enemyStatsTempalte.startingMaxHealth + lvl * enemyStatsTempalte.healthAddedOnPromotion));
                stats.AddStat(new Speed(enemyStatsTempalte.startingSpeed + lvl * enemyStatsTempalte.speedAddedOnPromotion));
                stats.AddStat(new AttackRange(enemyStatsTempalte.startingAttackRange + lvl * enemyStatsTempalte.attackRangeAddedOnPromotion));
                stats.AddStat(new SightRange(enemyStatsTempalte.startingSightRange + lvl * enemyStatsTempalte.sightRangeAddedOnPromotion));
                stats.AddStat(new MaximumHealth(enemyStatsTempalte.startingMaxHealth + lvl * enemyStatsTempalte.healthAddedOnPromotion));
                stats.AddStat(new Lvl(lvl));
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find GameplayController");
            }
        }
        private void Update()
        {
            if (alive && !pause && PlayerController.Instance != null)
            {
                currentPathfiningDirection = pathfindingUnit.currentDirection;

                SimpleAI();
                if (theAn.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    moving = false;
                }
                attackTimer += Time.deltaTime;
            }
        }
        private void FixedUpdate()
        {
            if (!pause && moving && (currentPathfiningDirection.x != 0 || currentPathfiningDirection.y != 0) && pathfindingUnit.pathPossible)
            {
                enemyRigidbody.velocity = currentPathfiningDirection * (float)stats.GetStat(typeof(Speed));
                theAn.SetFloat("X", currentPathfiningDirection.x);
                theAn.SetFloat("Y", currentPathfiningDirection.y);
                theAn.SetFloat("LastX", currentPathfiningDirection.x);
                theAn.SetFloat("LastY", currentPathfiningDirection.y);
            }
            else
            {
                theAn.SetFloat("X", 0);
                theAn.SetFloat("Y", 0);
                enemyRigidbody.velocity = Vector3.zero;
            }
        }
        void SimpleAI()
        {
            distanceFromPlayer = Vector3.Distance(GetCurrentEnemyPosition(), PlayerController.Instance.GetCurrentPlayerPosition());
            if (distanceFromPlayer <= stats.GetStat(typeof(SightRange)))
            {
                if (distanceFromPlayer > stats.GetStat(typeof(AttackRange)))
                {
                    //followPlayer
                    pathfindingUnit.active = true;
                    moving = true;
                }
                else
                {
                    //attackPlayer
                    pathfindingUnit.active = false;
                    moving = false;
                    if (attackTimer > 1 / stats.GetStat(typeof(AttackSpeed)))
                    {
                        Attack();
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                //doRandomStuff
                //TO DO
                pathfindingUnit.active = false;
                moving = false;
            }
        }
        virtual protected void Attack()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (alive)
            {
                if (collision.attachedRigidbody && collision.isTrigger)
                {
                    PlayerStatisticsController playerStatisticsController = collision.attachedRigidbody.GetComponentInParent<PlayerStatisticsController>();
                    if (playerStatisticsController)
                    {
                        double damageTaken = playerStatisticsController.DealDamage();
                        audioSource.Play();
                        TakeDamage(damageTaken);
                        ShowDamageTaken(damageTaken);
                        if (CheckIfKilled())
                        {
                            playerStatisticsController.GainExperimence(stats.GetStat(typeof(Lvl)) * 100);
                            Die();
                        }
                    }
                }
            }
        }
        private void ShowDamageTaken(double damageTaken)
        {
            GameObject popUp = Instantiate(damageTakenPopup, GetCurrentEnemyPosition() + Vector3.right * (Random.Range(-0.3f, 0.3f)) + Vector3.up * (Random.Range(1f, 2f)), Quaternion.identity, transform);
            popUp.GetComponentInChildren<PopupText>().ShowText(((int)damageTaken).ToString());
        }

        private void TakeDamage(double amountOfDamage)
        {
            stats.ChangeStatBase(typeof(Health), stats.GetStat(typeof(Health)) - amountOfDamage);
        }
        private bool CheckIfKilled()
        {
            if (stats.GetStat(typeof(Health)) < 0)
            {
                return true;
            }
            return false;
        }
        private void StartPause(OnPauseStart data)
        {
            enemyRigidbody.velocity = Vector3.zero;
            theAn.speed = 0;
            pause = true;
        }
        private void EndPause(OnPauseEnd data)
        {
            theAn.speed = 1;
            pause = false;
        }
        private void Die()
        {
            numberOfEnemies--;
            EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(numberOfEnemies));
            if (numberOfEnemies == 0)
            {
                EventManager.Instance.TriggerEvent(new OnLevelCompleted());
            }
            moving = false;
            alive = false;
            theAn.SetTrigger("Die");
            Destroy(gameObject, 0.6f);
        }
        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
                EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
            }
        }
    }
}
