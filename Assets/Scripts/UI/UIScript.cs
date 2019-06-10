using UnityEngine;

namespace MyRPGGame.UI
{
    public class UIScript : MonoBehaviour
    {
        private static UIScript instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void OnDestroy()
        {
            if(instance==this)
            {
                instance = null;
            }
        }
    }
}