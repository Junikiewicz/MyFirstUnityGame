using MyRPGGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPGGame.Collectables
{
    public class ExpBall : Collectable
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            PlayerStatisticsController playerStatisticsController = collision.GetComponent<PlayerStatisticsController>();
            if (playerStatisticsController)
            {
                audioSource.Play();
                spriteRenderer.enabled = false;
                playerStatisticsController.GainExperimence(value);
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }



}
