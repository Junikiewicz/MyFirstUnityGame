using MyRPGGame.Events;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class NewGameWindow : MenuItem
    {
        public InputField characterNameInput;
        public void StartNewGame()
        {
            if(characterNameInput.text!=string.Empty)
            {
                EventManager.Instance.TriggerEvent(new OnNewGame(characterNameInput.text));
                MainMenuController.Instance.CloseMainMenu();
            }
        }
    }

}
