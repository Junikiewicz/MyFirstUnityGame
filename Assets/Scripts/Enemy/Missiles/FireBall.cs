using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class FireBall : MonoBehaviour, IDamageDealer
    {
        public AudioClip FireBallStart;
        public AudioClip FireBallExplosion;
        private AudioSource audioSource;
        private Animator theAn;
        private Rigidbody2D fireballRigidbody;
        private double damage;
        private bool moving = true;
        private bool pause;
        private readonly float speed = 10;
        private float remainingLifeTime = 2f;
        void Awake()
        {
            fireballRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            theAn = GetComponent<Animator>();
            if (fireballRigidbody && audioSource && theAn)
            {
                audioSource.clip = FireBallStart;
                audioSource.Play();
                if (PlayerController.Instance)
                {
                    if (EventManager.Instance)
                    {
                        EventManager.Instance.AddListener<OnPauseStart>(StartPause);
                        EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
                    }
                    else
                    {
                        Debug.LogError(GetType() + " couldn't find EventManager.");
                    }
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find PlayerController");
                }
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find one of its required components");
                enabled = false;
            }
        }
        private void Update()
        {
            if (!pause)
            {
                remainingLifeTime -= Time.deltaTime;
                if (remainingLifeTime <= 0 && !theAn.GetCurrentAnimatorStateInfo(0).IsTag("Explosion"))
                {
                    DestroyFireball();
                }
            }
        }
        private void FixedUpdate()
        {
            if (!pause && moving && (PlayerController.Instance != null))
            {
                transform.up = transform.position - PlayerController.Instance.GetCurrentPlayerPosition();
                fireballRigidbody.velocity = (PlayerController.Instance.GetCurrentPlayerPosition() - transform.position).normalized * speed;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EnemyHitTrigger" && collision.gameObject.tag != "Missile")
            {
                moving = false;
                fireballRigidbody.velocity = Vector3.zero;

                if (collision.gameObject.tag == "Player")
                {
                    theAn.SetTrigger("Explosion");
                    audioSource.clip = FireBallExplosion;
                    audioSource.Play();
                    Invoke(nameof(DestroyFireball), FireBallExplosion.length);
                }
                else
                {
                    Invoke("DestroyFireball", 0);
                }
            }
        }
        public void SetDamage(double _damage)
        {
            damage = _damage;
        }
        public double DealDamage()
        {
            return damage;
        }
        void StartPause(OnPauseStart data)
        {
            fireballRigidbody.velocity = Vector3.zero;
            theAn.speed = 0;
            pause = true;
        }
        private void EndPause(OnPauseEnd data)
        {
            theAn.speed = 1;
            pause = false;
        }
        private void DestroyFireball()
        {
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if(EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
                EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
            }
        }
    }
}