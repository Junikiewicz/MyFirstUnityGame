using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRPGGame.World
{
    public class PortalController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                EventManager.Instance.TriggerEvent(new OnPortalEntered());
                SceneManager.LoadScene("Dungeon");
            }
        }
    }
}

