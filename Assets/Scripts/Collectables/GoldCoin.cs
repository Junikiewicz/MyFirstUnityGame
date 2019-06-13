using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Collectables
{
    public class GoldCoin : Collectable
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            PlayerStatisticsController playerStatisticsController = collision.GetComponent<PlayerStatisticsController>();
            if (playerStatisticsController)
            {
                audioSource.Play();
                spriteRenderer.enabled = false;
                playerStatisticsController.ChangeGold(value);
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }



}
