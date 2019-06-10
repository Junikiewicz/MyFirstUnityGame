using MyRPGGame.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    public class LoadMenuButton : MonoBehaviour
    {
        public Text playerName;
        public Text playerLvl;
        public Text worldLvl;
        public Text date;
        public Save saveData;

        public void OnClick()
        {
            SaveManager.Instance.LoadGame(saveData.playerName);
            MainMenuController.Instance.CloseMainMenu();
        }
        private void Awake()
        {
            if (!playerName || !playerLvl || !worldLvl || !date)
            {
                Debug.LogError(GetType() + " couldn't find one of its graphics components");
                enabled = false;
            }
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
