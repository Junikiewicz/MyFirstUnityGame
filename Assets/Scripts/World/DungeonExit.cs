using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRPGGame.World
{
    public class DungeonExit : MonoBehaviour
    {
        public string destinationScene;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                SceneManager.LoadScene(destinationScene);
                EventManager.Instance.TriggerEvent(new OnDungeonLeft());
            }
        }
    }

}
