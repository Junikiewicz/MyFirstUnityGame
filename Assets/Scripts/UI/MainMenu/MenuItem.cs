using UnityEngine;

namespace MyRPGGame.UI
{
    public abstract class MenuItem : MonoBehaviour
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
            MainMenuController.Instance.AddNewElementOnTop(this);
        }
    }
}
