using MyRPGGame.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class LoadMenuButton : MonoBehaviour
    {
        public Save saveData;

        [SerializeField] private Text playerName;
        [SerializeField] private Text playerLvl;
        [SerializeField] private Text worldLvl;
        [SerializeField] private Text date;

        public void OnClick()
        {
            SaveManager.Instance.LoadGame(saveData.playerName);
            MainMenuController.Instance.CloseMainMenu();
        }
        public void Inicialize(Save _saveData)
        {
            saveData = _saveData;
            playerLvl.text = (saveData.lvl).ToString();
            worldLvl.text = (saveData.worldLvl).ToString();
            playerName.text = (saveData.playerName).ToString();
            date.text = (saveData.date);
        }
    }

}
