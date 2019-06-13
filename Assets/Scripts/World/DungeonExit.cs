using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRPGGame.World
{
    public class DungeonExit : MonoBehaviour
    {
        [SerializeField] private string destinationScene="World";
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody)
            {
                if(collision.attachedRigidbody.GetComponent<PlayerController>())
                {
                    SceneManager.LoadScene(destinationScene);
                    EventManager.Instance.TriggerEvent(new OnDungeonLeft());
                }
            }
        }
    }

}
