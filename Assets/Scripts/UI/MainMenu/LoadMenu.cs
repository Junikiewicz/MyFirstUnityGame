using MyRPGGame.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPGGame.UI
{
    public class LoadMenu : MenuItem
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject contentList;
        public override void OpenMenuItem()
        {
            base.OpenMenuItem();
            List<Save> saveList = SaveManager.Instance.LoadSavesDate();
            for (int i = 0; i < saveList.Count; i++)
            {
                GameObject newLoadButton = Instantiate(buttonPrefab, contentList.transform);
                newLoadButton.GetComponent<LoadMenuButton>().Inicialize(saveList[i]);
            }
        }
        public override void Hide()
        {
            base.Hide();
            for (int i = 0; i < contentList.transform.childCount; i++)
            {
                Destroy(contentList.transform.GetChild(i).gameObject);
            }
        }
    }
}
