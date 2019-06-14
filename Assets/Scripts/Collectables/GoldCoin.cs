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
                GameObject popUp = Instantiate(goldPopup, transform.position + (Vector3)(Random.insideUnitCircle * 0.3f), Quaternion.identity, transform);
                int gold = Mathf.RoundToInt((float)(value + Random.Range((float)(-0.2 * value), (float)(0.2 * value))));
                popUp.GetComponentInChildren<PopupText>().ShowText("+"+(gold).ToString()+" g",Color.yellow,2);
                audioSource.Play();
                spriteRenderer.enabled = false;
                playerStatisticsController.ChangeGold(gold);
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }



}
