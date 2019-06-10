using UnityEngine;

namespace MyRPGGame.UI
{
    public abstract class MenuItem:MonoBehaviour
    {
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void OpenMenuItem()
        {
            if(MainMenuController.Instance)
            {
                MainMenuController.Instance.AddNewElementOnTop(this);
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find MainMenuController.");
            }
            
        }
    }
}
