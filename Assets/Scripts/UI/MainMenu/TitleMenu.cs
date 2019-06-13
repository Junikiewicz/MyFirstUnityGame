using UnityEngine;

namespace MyRPGGame.UI
{
    class TitleMenu : MenuItem
    {
        private static TitleMenu _instace;
        private void Start()
        {
            if (!_instace)
            {
                MainMenuController.Instance.defaulttMenuItem = this;
                MainMenuController.Instance.OpenMainMenu();
                _instace = this;
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }

        private void OnDestroy()
        {
            if (_instace == this)
            {
                _instace = null;
            }
        }
    }
}
