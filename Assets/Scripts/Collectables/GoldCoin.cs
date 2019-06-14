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
                float gold = (float)value;
                gold+= Mathf.RoundToInt(Random.Range(-0.2f *gold, 0.2f * gold));
                popUp.GetComponentInChildren<PopupText>().ShowText("+"+(gold).ToString()+" g",Color.yellow,2);
                audioSource.Play();
                spriteRenderer.enabled = false;
                playerStatisticsController.ChangeGold(gold);
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }



}
