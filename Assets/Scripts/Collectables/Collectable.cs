using MyRPGGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPGGame.Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        float speed = 5;
        bool moving;
        float lifeTime = 0;
        protected SpriteRenderer spriteRenderer;
        public double value;
        Rigidbody2D collectableRigidbody;
        Vector3 target;
        protected AudioSource audioSource;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            collectableRigidbody = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            lifeTime += Time.deltaTime;
            target = PlayerController.Instance.GetCurrentPlayerPosition();
            if (lifeTime>0.2f&&Vector3.Distance(transform.position,target)<5)
            {
                moving = true;
            }
            else
            {
                moving = false;
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
