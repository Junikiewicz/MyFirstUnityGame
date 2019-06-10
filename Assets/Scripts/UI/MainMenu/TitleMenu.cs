using UnityEngine;

namespace MyRPGGame.UI
{
    class TitleMenu:MenuItem
    {
        private static TitleMenu _instace;
        private void Start()
        {
            if(!_instace)
            {
                if(MainMenuController.Instance)
                {
                    MainMenuController.Instance.defaulttMenuItem = this;
                    MainMenuController.Instance.OpenMainMenu();
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find MainMenuController.");
                }
                _instace = this;
            }
        }

        private void OnDestroy()
        {
            if(_instace==this)
            {
                _instace = null;
            }
        }
    }
}
