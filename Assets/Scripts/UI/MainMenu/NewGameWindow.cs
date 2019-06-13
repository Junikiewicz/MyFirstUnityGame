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
            EventManager.Instance.TriggerEvent(new OnNewGame(characterNameInput.text));
            MainMenuController.Instance.CloseMainMenu();
        }
    }

}
