using UnityEngine;
using MyRPGGame.Statistic;
using MyRPGGame.Enemies;
using MyRPGGame.Events;

namespace MyRPGGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AudioSource gotHit;
        [SerializeField] private AudioSource heavyBreathing;
        [SerializeField] private AudioSource death;

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

        //input
        private const string SprintButton = "Sprint";
        private const string AttackButton = "Attack";
        private const string InputHorizontalAxis = "Horizontal";
        private const string InputVerticalAxis = "Vertical";

        //animator
        private const string AnimatorDeathTrigger = "Death";
        private const string AttackAnimationTag = "Attack";
        private const string SprintAnimatorParametr = "Running";
        private const string XAnimatorParametr = "X";
        private const string YAnimatorParametr = "Y";
        private const string LastXAnimatorParametr = "LastX";
        private const string LastYAnimatorParametr = "LastY";
        private const string AttackBufferAnimatorParametr = "AttackBuffor";


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

                EventManager.Instance.AddListener<OnPlayerKilled>(PlayerKillied);
                EventManager.Instance.AddListener<OnPlayerExhausted>(PlayerExhaustion);
                EventManager.Instance.AddListener<OnPauseStart>(StartPause);
                EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
                EventManager.Instance.AddListener<OnPlayerTeleportation>(TeleportPlayer);
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
            if (collision.attachedRigidbody && collision.isTrigger)
            {
                IDamageDealer damageDealer = collision.attachedRigidbody.GetComponent<IDamageDealer>();
                if (damageDealer != null)
                {
                    playerStats.ChangeHealth(-damageDealer.DealDamage());
                    gotHit.Play();
                    EventManager.Instance.TriggerEvent(new OnPlayerHit());
                }
            }
        }

        private void PlayerMovement()
        {
            if (!attacking)
            {
                velocity.x = Input.GetAxisRaw(InputHorizontalAxis) * (float)playerStats.GetPlayerStat(typeof(Speed));
                velocity.y = Input.GetAxisRaw(InputVerticalAxis) * (float)playerStats.GetPlayerStat(typeof(Speed));
            }
            else
            {
                velocity.x = 0;
                velocity.y = 0;
            }
            if (Input.GetButton(SprintButton) && !exhausted && (velocity.x != 0 || velocity.y != 0))
            {
                sprinting = true;
                playerStats.ChangeStamina(-4);
                velocity *= 2;
            }
            else
            {
                sprinting = false;
            }

            playerAnimator.SetBool(SprintAnimatorParametr, sprinting);
            playerAnimator.SetFloat(YAnimatorParametr, velocity.y);
            playerAnimator.SetFloat(XAnimatorParametr, velocity.x);
            if (velocity.x != 0 || velocity.y != 0)
            {
                playerAnimator.SetFloat(LastXAnimatorParametr, velocity.x);
                playerAnimator.SetFloat(LastYAnimatorParametr, velocity.y);
            }
        }
        private void PlayerAttack()
        {
            timeDelayForHoldingSpace += Time.deltaTime;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag(AttackAnimationTag))
            {
                attacking = true;
            }
            else
            {
                attacking = false;
                attackBuffor = 0;
            }
            if ((Input.GetButtonDown(AttackButton) || (Input.GetButton(AttackButton) && timeDelayForHoldingSpace > 0.3f)) &&
              playerStats.GetPlayerStat(typeof(Stamina)) > 50 &&
              attackBuffor < 3)
            {
                playerStats.ChangeStamina(-50);
                attackBuffor++;
                timeDelayForHoldingSpace = 0;
            }
            playerAnimator.SetInteger(AttackBufferAnimatorParametr, attackBuffor);
        }
        private void StartPause(OnPauseStart eventData)
        {
            playerRigibody2D.velocity = Vector3.zero;
            pause = true;
        }
        private void EndPause(OnPauseEnd eventData)
        {
            pause = false;
        }
        private void PlayerExhaustion(OnPlayerExhausted eventData)
        {
            exhausted = true;
            heavyBreathing.Play();
            Invoke(nameof(PlayerRested), 2);
        }
        private void PlayerRested()
        {
            exhausted = false;
        }
        private void PlayerKillied(OnPlayerKilled eventData)
        {
            playerAnimator.SetTrigger(AnimatorDeathTrigger);
            death.Play();
        }
        private void TeleportPlayer(OnPlayerTeleportation eventData)
        {
            transform.position = eventData.destination;
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                EventManager.Instance.RemoveListener<OnPlayerExhausted>(PlayerExhaustion);
                EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
                EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
                EventManager.Instance.RemoveListener<OnPlayerKilled>(PlayerKillied);
                EventManager.Instance.RemoveListener<OnPlayerTeleportation>(TeleportPlayer);
            }
        }
    }
}