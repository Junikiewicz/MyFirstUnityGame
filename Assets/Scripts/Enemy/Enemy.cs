using UnityEngine;
using MyRPGGame.Statistics;
using MyRPGGame.PathFinding;
using MyRPGGame.Player;
using MyRPGGame.Events;
using MyRPGGame.Templates;
using MyRPGGame.Collectables;
using System.Collections;

namespace MyRPGGame.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageDealer
    {
        public static int numberOfEnemies = 0;
        public EnemyClass characterClass;

        [SerializeField] private EnemyStatsTemplate enemyStatsTempalte;
        [SerializeField] private GameObject damageTakenPopup;
        [SerializeField] private GameObject goldCoin;
        [SerializeField] private GameObject expBall;

        protected StatBlock stats;
        protected Animator theAn;

        private AudioSource audioSource;
        private Rigidbody2D enemyRigidbody;
        private Unit pathfindingUnit;

        private Vector3 currentDirection;
        private double distanceFromPlayer;

        private bool attacking;
        private bool alive = true;
        private bool moving = false;
        private bool randomRunning = false;
        private float attackTimer = 0;

        protected bool pause = false;

        private int animatorX;
        private int animatorY;
        private const string AttackAnimationTag = "Attack";
        private const string XAnimatorParametr = "X";
        private const string YAnimatorParametr = "Y";
        private const string LastXAnimatorParametr = "LastX";
        private const string LastYAnimatorParametr = "LastY";

        public Vector3 GetCurrentEnemyPosition()//hotfix due to wrongly placed pivot in 500+ sprites. Will be fixed as soon as i get sorting groups right.
        {
            return new Vector3(transform.position.x, transform.position.y - 1f, 0);
        }
        public double GetStat(Stat stat)
        {
            return stats.GetStat(stat);
        }

        public double DealDamage()//used by player
        {
            double basicDamage = stats.GetStat(Stat.AttackDamage);
            return basicDamage+Random.Range((float)(-0.2*basicDamage),(float)(0.2*basicDamage));
        }

        protected virtual void Awake()
        {
            enemyRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            stats = gameObject.AddComponent<StatBlock>();
            theAn = GetComponent<Animator>();
            pathfindingUnit = GetComponent<Unit>();
            if (PlayerController.Instance)
            {
                pathfindingUnit.Inicialize(PlayerController.Instance.GetCurrentPlayerPosition, GetCurrentEnemyPosition);
            }
            EventManager.Instance.AddListener<OnPauseStart>(StartPause);
            EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
            numberOfEnemies++;
            EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(numberOfEnemies));
        }
        protected virtual void Start()
        {
            int lvl = GameplayController.Instance.currentWorldLevel;
            stats.AddStat(new Statistic(Stat.AttackDamage,enemyStatsTempalte.startingAttackDamage + lvl * enemyStatsTempalte.attackDamageAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.AttackSpeed,enemyStatsTempalte.startingAttackSpeed + lvl * enemyStatsTempalte.attackSpeedAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.Health,enemyStatsTempalte.startingMaxHealth + lvl * enemyStatsTempalte.healthAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.Speed,enemyStatsTempalte.startingSpeed + lvl * enemyStatsTempalte.speedAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.AttackRange,enemyStatsTempalte.startingAttackRange + lvl * enemyStatsTempalte.attackRangeAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.SightRange,enemyStatsTempalte.startingSightRange + lvl * enemyStatsTempalte.sightRangeAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.MaximumHealth,enemyStatsTempalte.startingMaxHealth + lvl * enemyStatsTempalte.healthAddedOnPromotion));
            stats.AddStat(new Statistic(Stat.Lvl,lvl));
        }
        private void Update()
        {
            if(alive&&!pause)
            {
                attacking = theAn.GetCurrentAnimatorStateInfo(0).IsTag(AttackAnimationTag);
                if (!attacking)
                {
                    if (pathfindingUnit.active)
                    {
                        currentDirection = pathfindingUnit.currentDirection;
                    }
                    SimpleAI();
                }
                SetAnimations();
                attackTimer += Time.deltaTime;
            }
        }

        private void SimpleAI()
        {
            distanceFromPlayer = Vector3.Distance(GetCurrentEnemyPosition(), PlayerController.Instance.GetCurrentPlayerPosition());
            if (distanceFromPlayer <= stats.GetStat(Stat.SightRange))
            {
                if (distanceFromPlayer <= stats.GetStat(Stat.AttackRange))
                {
                    //attackPlayer
                    moving = false;
                    if (attackTimer > 1 / stats.GetStat(Stat.AttackSpeed))
                    {
                        Attack();
                        attackTimer = 0;
                    }
                }
                else
                {
                    //followPlayer
                    moving = true;
                    pathfindingUnit.active = true;
                }
            }
            else
            {
                //doRandomStuff
                pathfindingUnit.active = false;
                if (!randomRunning)
                {
                    StartCoroutine(RandomBehaviour());
                }
            }
        }
        private void SetAnimations()
        {
            if (Mathf.Abs(currentDirection.x) > Mathf.Abs(currentDirection.y))
            {
                animatorX = currentDirection.x > 0 ? 1 : -1;
                animatorY = 0;
            }
            else
            {
                animatorX = 0;
                animatorY = currentDirection.y > 0 ? 1 : -1;
            }
            theAn.SetFloat(LastXAnimatorParametr, animatorX);
            theAn.SetFloat(LastYAnimatorParametr, animatorY);
            if (moving)
            {
                theAn.SetFloat(XAnimatorParametr, animatorX);
                theAn.SetFloat(YAnimatorParametr, animatorY);
            }
            else
            {
                theAn.SetFloat(XAnimatorParametr, 0);
                theAn.SetFloat(YAnimatorParametr, 0);
            }
        }

        private void FixedUpdate()
        {
            if (moving&&!attacking)
            {
                enemyRigidbody.velocity = currentDirection * (float)stats.GetStat(Stat.Speed);
            }
            else
            {
                enemyRigidbody.velocity = Vector3.zero;
            }
        }

        IEnumerator RandomBehaviour()
        {
            randomRunning = true;
            while (true)
            {
                if(pathfindingUnit.active||pause)
                {
                    randomRunning = false;
                    yield break;
                }
                currentDirection = Random.insideUnitCircle;
                if(moving)
                {
                    moving = false;
                    yield return new WaitForSeconds(Random.Range(4, 5));
                }
                else
                {
                    moving = true;
                    yield return new WaitForSeconds(Random.Range(1, 2));
                }
            }
        }
        protected abstract void Attack();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (alive)
            {
                if (collision.attachedRigidbody && collision.isTrigger)
                {
                    PlayerStatisticsController playerStatisticsController = collision.attachedRigidbody.GetComponent<PlayerStatisticsController>();
                    if (playerStatisticsController)
                    {
                        double damageTaken = playerStatisticsController.DealDamage();
                        audioSource.Play();
                        TakeDamage(damageTaken);
                        ShowDamageTaken(damageTaken);
                        if (CheckIfKilled())
                        {
                            Die();
                        }
                    }
                }
            }
        }
        private void ShowDamageTaken(double damageTaken)
        {
            GameObject popUp = Instantiate(damageTakenPopup, transform.position + (Vector3)(Random.insideUnitCircle * 0.3f), Quaternion.identity, transform);
            popUp.GetComponentInChildren<PopupText>().ShowText(((int)damageTaken).ToString(), Color.white, 3);
        }

        private void TakeDamage(double amountOfDamage)
        {
            stats.ChangeStatBase(Stat.Health, stats.GetStat(Stat.Health) - amountOfDamage);
        }
        private bool CheckIfKilled()
        {
            if (stats.GetStat(Stat.Health) < 0)
            {
                return true;
            }
            return false;
        }
        private void Die()
        {
            DropItems(goldCoin, Random.Range(4, 8));
            DropItems(expBall, Random.Range(4, 8), 10);
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
        private void DropItems(GameObject item, int amountOfItems, double valueMultiplicator = 1)
        {
            for (int i = 0; i < amountOfItems; i++)
            {
                var spawnedItem = Instantiate(item, transform.position + (Vector3)Random.insideUnitCircle * 1.5f, Quaternion.identity);
                spawnedItem.GetComponent<Collectable>().value = stats.GetStat(Stat.Lvl) * valueMultiplicator;
            }
        }

        private void StartPause(OnPauseStart eventData)
        {
            enemyRigidbody.velocity = Vector3.zero;
            theAn.speed = 0;
            pause = true;
        }
        private void EndPause(OnPauseEnd eventData)
        {
            theAn.speed = 1;
            pause = false;
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
            EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
        }
    }
}
