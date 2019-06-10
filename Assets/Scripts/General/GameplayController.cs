using MyRPGGame.Enemies;
using MyRPGGame.Events;
using MyRPGGame.Player;
using MyRPGGame.SaveSystem;
using MyRPGGame.World;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRPGGame
{
    public class GameplayController : MonoBehaviour
    {
        public static GameplayController Instance { get; private set; }
        public string playerName;
        public int currentWorldLevel = 1;
        private bool gameEnded = false;
        public GameObject portal;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                if(EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnPlayerKilled>(PlayerDeath);
                    EventManager.Instance.AddListener<OnLevelCompleted>(AllEnemiesKilled);
                    EventManager.Instance.AddListener<OnPortalEntered>(EnterPortal);
                    EventManager.Instance.AddListener<OnDungeonLeft>(LeaveDungeon);
                    EventManager.Instance.AddListener<OnNewGame>(StartNewGame);
                    EventManager.Instance.AddListener<OnGameLoaded>(LoadDataFromSave);
                    EventManager.Instance.AddListener<OnGameSaved>(AddAdditionalDataToSave);
                }
                else
                {
                    Debug.LogError(GetType() + " couldn't find EventManager.");
                }
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void StartNewGame(OnNewGame Data)
        {
            gameEnded = false;
            SceneManager.LoadScene("Dungeon");
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
            playerName =Data.CharacterName;
            currentWorldLevel = 1;
            Enemy.numberOfEnemies = 0;
            EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(0));
            EventManager.Instance.TriggerEvent(new OnPauseEnd());
        }
        private void LeaveDungeon(OnDungeonLeft Data)
        {
            Invoke(nameof(CreateWorld), 0.1f);
        }
        private void CreateWorld()
        {
            WorldGeneration.Instance.StartGeneratingWorld(30, currentWorldLevel * 10);
        }
        private void AllEnemiesKilled(OnLevelCompleted Data)
        {
            if (!gameEnded)
            {
                Vector3 temp = PlayerController.Instance.transform.position;
                temp.y += 5;
                Instantiate(portal, temp, Quaternion.identity);
            }
        }
        private void EnterPortal(OnPortalEntered Data)
        {
            Enemy.numberOfEnemies = 0;
            currentWorldLevel++;
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
            SaveManager.Instance.SaveGame(playerName);
        }
        private void PlayerDeath(OnPlayerKilled Data)
        {
            gameEnded = true;
            EventManager.Instance.TriggerEvent(new OnPauseStart());
        }
        private void AddAdditionalDataToSave(OnGameSaved data)
        {
            data.saveData.worldLvl = currentWorldLevel;
            data.saveData.date = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            data.saveData.playerName = playerName;
        }

        private void LoadDataFromSave(OnGameLoaded data)
        {
            EventManager.Instance.TriggerEvent(new OnPauseEnd());
            currentWorldLevel = data.saveData.worldLvl;
            playerName = data.saveData.playerName;

            gameEnded = false;
            SceneManager.LoadScene("Dungeon");
            Enemy.numberOfEnemies = 0;
            EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(0));
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                if (EventManager.Instance)
                {
                    EventManager.Instance.RemoveListener<OnPlayerKilled>(PlayerDeath);
                    EventManager.Instance.RemoveListener<OnLevelCompleted>(AllEnemiesKilled);
                    EventManager.Instance.RemoveListener<OnPortalEntered>(EnterPortal);
                    EventManager.Instance.RemoveListener<OnDungeonLeft>(LeaveDungeon);
                    EventManager.Instance.RemoveListener<OnNewGame>(StartNewGame);
                    EventManager.Instance.RemoveListener<OnGameLoaded>(LoadDataFromSave);
                    EventManager.Instance.RemoveListener<OnGameSaved>(AddAdditionalDataToSave);
                }
            }
        }
    }

}
