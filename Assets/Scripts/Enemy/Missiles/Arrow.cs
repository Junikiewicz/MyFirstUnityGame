using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class Arrow : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private AudioClip ArrowStart;
        [SerializeField] private AudioClip ArrowHit;
        [SerializeField] private float speed = 10;
        [SerializeField] private float targetRadius = 0.1f;

        private AudioSource audioSource;
        private Rigidbody2D arrowRigidbody;

        private Vector3 target;
        private double damage;
        private bool moving = true;
        private bool pause = false;

        private void Awake()
        {
            arrowRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = ArrowStart;
            audioSource.Play();

            target = PlayerController.Instance.GetCurrentPlayerPosition();

            transform.up = transform.position - target;

            EventManager.Instance.AddListener<OnPauseStart>(StartPause);
            EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
        }

        private void Update()
        {
            if (Vector3.Distance(target, transform.position) < targetRadius)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (moving && !pause)
            {
                arrowRigidbody.velocity = (target - transform.position).normalized * speed;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.attachedRigidbody)
            {
                if(collision.attachedRigidbody.GetComponent<PlayerController>())
                {
                    moving = false;
                    arrowRigidbody.velocity = Vector3.zero;
                    audioSource.clip = ArrowHit;
                    audioSource.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    Destroy(gameObject, ArrowHit.length);
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
        private void StartPause(OnPauseStart eventData)
        {
            arrowRigidbody.velocity = Vector3.zero;
            pause = true;
        }
        private void EndPause(OnPauseEnd eventData)
        {
            pause = false;
        }
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
            EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
        }
    }
}