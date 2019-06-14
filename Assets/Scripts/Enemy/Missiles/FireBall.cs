using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class FireBall : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private AudioClip FireBallStart;
        [SerializeField] private AudioClip FireBallExplosion;
        [SerializeField] private float speed = 10;

        private AudioSource audioSource;
        private Animator theAn;
        private Rigidbody2D fireballRigidbody;

        private const string ExplosionTrigger = "Explosion";
        private const string ExplosionAnimationTag = "Explosion";
        private double damage;
        private bool moving = true;
        private bool pause;
        
        private float remainingLifeTime = 2f;
        void Awake()
        {
            fireballRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            theAn = GetComponent<Animator>();
            audioSource.clip = FireBallStart;
            audioSource.Play();
            EventManager.Instance.AddListener<OnPauseStart>(StartPause);
            EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
        }
        private void Update()
        {
            if (!pause)
            {
                remainingLifeTime -= Time.deltaTime;
                if (remainingLifeTime <= 0 && !theAn.GetCurrentAnimatorStateInfo(0).IsTag(ExplosionAnimationTag))
                {

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
            if (collision.attachedRigidbody)
            {
                if (collision.attachedRigidbody.GetComponent<PlayerController>())
                {
                    moving = false;
                    fireballRigidbody.velocity = Vector3.zero;
                    theAn.SetTrigger(ExplosionTrigger);
                    GetComponent<Collider2D>().enabled = false;
                    audioSource.clip = FireBallExplosion;
                    audioSource.Play();
                    Destroy(gameObject, FireBallExplosion.length);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void SetDamage(double _damage)
        {
            damage = _damage;
        }
        public double DealDamage()
        {
            return damage + Random.Range((float)(-0.2 * damage), (float)(0.2 * damage));
        }
        void StartPause(OnPauseStart eventData)
        {
            fireballRigidbody.velocity = Vector3.zero;
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