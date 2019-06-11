using UnityEngine;
using MyRPGGame.Statistic;
using MyRPGGame.Enemies;
using MyRPGGame.Events;

namespace MyRPGGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public AudioSource gotHit;
        public AudioSource heavyBreathing;
        public AudioSource death;

        private PlayerStatisticsController playerStats;
        private Rigidbody2D playerRigibody2D;
        private Animator playerAnimator;

        private bool sprinting;
        private bool exhausted;
        private bool attacking;
        private int attackBuffor;
        private Vector2 velocity;
        private float timeDelayForHoldingSpace;
        private bool pause = true;
        public static PlayerController Instance { get; private set; }
        public Vector3 GetCurrentPlayerPosition()//hotfix due to wrongly placed pivot in 500+ sprites. Will be fixed as soon as i get sorting groups right.
        {
            return new Vector3(transform.position.x, transform.position.y - 1f, 0);
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                playerStats = GetComponent<PlayerStatisticsController>();
                playerAnimator = GetComponent<Animator>();
                playerRigibody2D = GetComponent<Rigidbody2D>();
                if (playerRigibody2D && playerStats && playerAnimator)
                {
                    if (EventManager.Instance)
                    {
                        EventManager.Instance.AddListener<OnPlayerKilled>(PlayerKillied);
                        EventManager.Instance.AddListener<OnPlayerExhausted>(PlayerExhaustion);
                        EventManager.Instance.AddListener<OnPauseStart>(StartPause);
                        EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
                        EventManager.Instance.AddListener<OnPlayerTeleportation>(TeleportPlayer);
                    }
                    else
                    {
                        Debug.LogError(GetType() + " couldn't find EventManager.");
                    }
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find one of its required components");
                    enabled = false;
                }
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void Update()
        {
            if (!pause)
            {
                PlayerMovement();
                PlayerAttack();
            }
        }
        private void FixedUpdate()
        {
            if (!pause)
            {
                playerRigibody2D.velocity = velocity;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageDealer damageDealer = collision.attachedRigidbody.GetComponent<IDamageDealer>();
            if (damageDealer!=null)
            {
                playerStats.ChangeHealth(-damageDealer.DealDamage());
                gotHit.Play();
                EventManager.Instance.TriggerEvent(new OnPlayerHit());
            }
        }

        private void PlayerMovement()
        {
            if (!attacking)
            {
                velocity.x = Input.GetAxisRaw("Horizontal") * (float)playerStats.GetPlayerStat(typeof(Speed));
                velocity.y = Input.GetAxisRaw("Vertical") * (float)playerStats.GetPlayerStat(typeof(Speed));
            }
            else
            {
                velocity.x = 0;
                velocity.y = 0;
            }
            if (Input.GetButton("Sprint") && !exhausted && (velocity.x != 0 || velocity.y != 0))
            {
                sprinting = true;
                playerStats.ChangeStamina(-4);
                velocity *= 2;
            }
            else
            {
                sprinting = false;
            }

            playerAnimator.SetBool("Running", sprinting);
            playerAnimator.SetFloat("Y", velocity.y);
            playerAnimator.SetFloat("X", velocity.x);
            if (velocity.x != 0 || velocity.y != 0)
            {
                playerAnimator.SetFloat("LastX", velocity.x);
                playerAnimator.SetFloat("LastY", velocity.y);
            }
        }
        private void PlayerAttack()
        {
            timeDelayForHoldingSpace += Time.deltaTime;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                attacking = true;
            }
            else
            {
                attacking = false;
                attackBuffor = 0;
            }
            if ((Input.GetButtonDown("Attack") || (Input.GetButton("Attack") && timeDelayForHoldingSpace > 0.3f)) &&
              playerStats.GetPlayerStat(typeof(Stamina)) > 50 &&
              attackBuffor < 3)
            {
                playerStats.ChangeStamina(-50);
                attackBuffor++;
                timeDelayForHoldingSpace = 0;
            }
            playerAnimator.SetInteger("AttackBuffor", attackBuffor);
        }
        private void StartPause(OnPauseStart data)
        {
            playerRigibody2D.velocity = Vector3.zero;
            pause = true;
        }
        private void EndPause(OnPauseEnd data)
        {
            pause = false;
        }
        private void PlayerExhaustion(OnPlayerExhausted Data)
        {
            exhausted = true;
            heavyBreathing.Play();
            Invoke("PlayerRested", 2);
        }
        private void PlayerRested()
        {
            exhausted = false;
        }
        private void PlayerKillied(OnPlayerKilled data)
        {
            playerAnimator.SetTrigger("Death");
            death.Play();
        }
        private void TeleportPlayer(OnPlayerTeleportation data)
        {
            transform.position = data.destination;
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                if (EventManager.Instance)
                {
                    EventManager.Instance.RemoveListener<OnPlayerExhausted>(PlayerExhaustion);
                    EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
                    EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
                    EventManager.Instance.RemoveListener<OnPlayerKilled>(PlayerKillied);
                    EventManager.Instance.RemoveListener<OnPlayerTeleportation>(TeleportPlayer);
                }
            }
        }
    }
}