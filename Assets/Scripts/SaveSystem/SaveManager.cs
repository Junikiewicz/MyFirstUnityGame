using MyRPGGame.Events;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MyRPGGame.SaveSystem
{
    class SaveManager : ScriptableObject
    {
        private static SaveManager m_instance = null;
        public static SaveManager Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = FindObjectOfType<SaveManager>();
                }
                if (!m_instance)
                {
                    m_instance = CreateInstance<SaveManager>();
                }
                return m_instance;
            }
        }
        public void SaveGame(string fileName)
        {
            Save save = new Save();
            EventManager.Instance.TriggerEvent(new OnGameSaved(save));
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/saves/" + fileName + ".save");
            bf.Serialize(file, save);
            file.Close();

            Debug.Log("Game Saved");
        }
        public void LoadGame(string fileName)
        {
            if (File.Exists(Application.persistentDataPath + "/saves/" + fileName + ".save"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/saves/" + fileName + ".save", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();
                EventManager.Instance.TriggerEvent(new OnGameLoaded(save));
                Debug.Log("Game Loaded");
            }
            else
            {
                Debug.Log("No game saved!");
            }
        }
        public List<Save> LoadSavesDate()
        {
            BinaryFormatter bf = new BinaryFormatter();
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/saves", "*.save");
            List<Save> SaveList = new List<Save>();
            for (int i = 0; i < filePaths.Length; i++)
            {
                FileStream file = File.Open(filePaths[i], FileMode.Open);
                SaveList.Add((Save)bf.Deserialize(file));
                file.Close();
            }
            return SaveList;
        }
        private void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
    }
}
