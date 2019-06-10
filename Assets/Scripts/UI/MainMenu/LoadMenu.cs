using MyRPGGame.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPGGame.UI
{
    public class LoadMenu : MenuItem
    {
        public GameObject buttonPrefab;
        public GameObject contentList;

        private void Awake()
        {
            if (!buttonPrefab || !contentList)
            {
                Debug.LogError(GetType() + " couldn't find one of its required components");
                enabled = false;
            }
        }
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
