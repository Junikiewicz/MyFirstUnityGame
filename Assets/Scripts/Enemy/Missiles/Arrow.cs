using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public class Arrow : MonoBehaviour, IDamageDealer
    {
        public AudioClip ArrowStart;
        public AudioClip ArrowHit;
        private AudioSource audioSource;
        private Vector3 target;
        private double damage;
        private Rigidbody2D arrowRigidbody;
        private bool moving = true;
        private bool pause = false;
        private readonly float speed = 10;
        private readonly float targetRadius = 0.1f;

        private void Awake()
        {
            arrowRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            if (arrowRigidbody && audioSource)
            {
                audioSource.clip = ArrowStart;
                audioSource.Play();    
                if (PlayerController.Instance)
                {
                    target = PlayerController.Instance.GetCurrentPlayerPosition();
                    transform.up = transform.position - target;
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
                    enabled = false;
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
            if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Missile")
            {
                moving = false;
                arrowRigidbody.velocity = Vector3.zero;

                if (collision.gameObject.tag == "Player")
                {
                    audioSource.clip = ArrowHit;
                    audioSource.Play();
                    GetComponent<SpriteRenderer>().sprite = null;
                    Destroy(gameObject, ArrowHit.length);
                }
                else
                {
                    Destroy(gameObject);
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
        private void StartPause(OnPauseStart data)
        {
            arrowRigidbody.velocity = Vector3.zero;
            pause = true;
        }
        private void EndPause(OnPauseEnd data)
        {
            pause = false;
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