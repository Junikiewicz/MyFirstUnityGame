using MyRPGGame.Enemies;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Collectables
{
    public class GoldCoin : Collectable
    {
        [SerializeField] private GameObject goldPopup;
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            PlayerStatisticsController playerStatisticsController = collision.GetComponent<PlayerStatisticsController>();
            if (playerStatisticsController)
            {
                GameObject popUp = Instantiate(goldPopup, transform.position + Vector3.right * (Random.Range(-0.3f, 0.3f)) + Vector3.up * (Random.Range(1f, 2f)), Quaternion.identity, transform);
                popUp.GetComponentInChildren<PopupText>().ShowText("+"+(value).ToString()+" g",Color.yellow,2);
                audioSource.Play();
                spriteRenderer.enabled = false;
                playerStatisticsController.ChangeGold(value);
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }



}
