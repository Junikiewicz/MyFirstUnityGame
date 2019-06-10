using MyRPGGame.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class NewGameWindow : MenuItem
    {
        public InputField characterNameInput;
        public void StartNewGame()
        {
            if(EventManager.Instance)
            {
                EventManager.Instance.TriggerEvent(new OnNewGame(characterNameInput.text));
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find EventManager.");
            }

            if (MainMenuController.Instance)
            {
                MainMenuController.Instance.CloseMainMenu();
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find MainMenuController.");
            }

        }
    }

}
