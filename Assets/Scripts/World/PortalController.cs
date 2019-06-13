using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRPGGame.World
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField]private string destinationScene = "Dungeon";
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody)
            {
                if (collision.attachedRigidbody.GetComponent<PlayerController>())
                {
                    EventManager.Instance.TriggerEvent(new OnPortalEntered());
                    SceneManager.LoadScene(destinationScene);
                }
            }
        }
    }
}

