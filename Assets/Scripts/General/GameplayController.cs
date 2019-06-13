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
        [SerializeField] private GameObject portal;
        [SerializeField] private string startingScene = "Dungeon";
        private const string dateFormatForSave = "dd-MM-yyyy HH:mm:ss";

        private string playerName;
        public int currentWorldLevel = 1;
        private bool gameEnded = false;
        public static GameplayController Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
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
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void StartNewGame(OnNewGame eventData)
        {
            gameEnded = false;
            SceneManager.LoadScene(startingScene);
            playerName = eventData.CharacterName;
            currentWorldLevel = 1;
            Enemy.numberOfEnemies = 0;
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
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
        private void AllEnemiesKilled(OnLevelCompleted eventData)
        {
            if (!gameEnded)
            {
                Vector3 temp = PlayerController.Instance.transform.position;
                temp.y += 5;
                Instantiate(portal, temp, Quaternion.identity);
            }
        }
        private void EnterPortal(OnPortalEntered eventData)
        {
            Enemy.numberOfEnemies = 0;
            currentWorldLevel++;
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
            SaveManager.Instance.SaveGame(playerName);
        }
        private void PlayerDeath(OnPlayerKilled eventData)
        {
            gameEnded = true;
            EventManager.Instance.TriggerEvent(new OnPauseStart());
        }
        private void AddAdditionalDataToSave(OnGameSaved eventData)
        {
            eventData.saveData.worldLvl = currentWorldLevel;
            eventData.saveData.date = System.DateTime.Now.ToString(dateFormatForSave);
            eventData.saveData.playerName = playerName;
        }
        private void LoadDataFromSave(OnGameLoaded eventData)
        {
            EventManager.Instance.TriggerEvent(new OnPauseEnd());
            currentWorldLevel = eventData.saveData.worldLvl;
            playerName = eventData.saveData.playerName;

            gameEnded = false;
            SceneManager.LoadScene(startingScene);
            Enemy.numberOfEnemies = 0;
            EventManager.Instance.TriggerEvent(new OnNumberOfEnemiesChanged(0));
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(new Vector3(0, 11, 0)));
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
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
