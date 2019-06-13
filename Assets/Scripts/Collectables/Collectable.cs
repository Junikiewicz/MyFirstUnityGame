using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        public double value;

        [SerializeField]private float speed = 5;
        [SerializeField]private float inactivityTimer = 0.2f;

        private bool moving;
        private Vector3 target;

        protected AudioSource audioSource;
        private Rigidbody2D collectableRigidbody;
        protected SpriteRenderer spriteRenderer;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            collectableRigidbody = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            if(inactivityTimer>0)
            {
                inactivityTimer -= Time.deltaTime;
            }
            else
            {
                target = PlayerController.Instance.GetCurrentPlayerPosition();
                if (Vector3.Distance(transform.position, target) < 5)
                {
                    moving = true;
                }
                else
                {
                    moving = false;
                }
            }
        }
        private void FixedUpdate()
        {
            if(moving)
            {
                collectableRigidbody.velocity = (target - transform.position).normalized*speed;
            }
            else
            {
                collectableRigidbody.velocity = Vector3.zero;
            }
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
           
        }
    }

}
