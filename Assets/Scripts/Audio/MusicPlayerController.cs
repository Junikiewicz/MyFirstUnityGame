using MyRPGGame.Events;
using UnityEngine;

namespace MyRPGGame.Audio
{
    public class MusicPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip fightMusic,dungeonMusic,menuMusic;

        private AudioSource musicSpeaker;
        private bool isPlayerInDungeon = true;
        private static MusicPlayerController _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                musicSpeaker = GetComponent<AudioSource>();

                EventManager.Instance.AddListener<OnDungeonLeft>(StartFightMusic);
                EventManager.Instance.AddListener<OnPortalEntered>(StartDungeonMusic);
                EventManager.Instance.AddListener<OnMenuOpened>(StartMenuMusic);
                EventManager.Instance.AddListener<OnMenuClosed>(ResumeMusicAfterMenuClosed);
                EventManager.Instance.AddListener<OnGameLoaded>(StartDungeonMusic);
                EventManager.Instance.AddListener<OnLevelCompleted>(StopMusic);
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void ResumeMusicAfterMenuClosed(OnMenuClosed data)
        {
            if (isPlayerInDungeon)
            {
                musicSpeaker.clip = dungeonMusic;
                musicSpeaker.PlayDelayed(2);
                musicSpeaker.loop = true;
            }
            else
            {
                musicSpeaker.clip = fightMusic;
                musicSpeaker.Play();
            }
        }
        private void StartMenuMusic(OnMenuOpened data)
        {
            musicSpeaker.clip = menuMusic;
            musicSpeaker.Play();
        }
        private void StartFightMusic(OnDungeonLeft data)
        {
            musicSpeaker.clip = fightMusic;
            isPlayerInDungeon = false;
            musicSpeaker.Play();
        }
        private void StopMusic(OnLevelCompleted data)
        {
            musicSpeaker.Stop();
        }
        private void StartDungeonMusic(OnGameLoaded data)
        {
            musicSpeaker.clip = dungeonMusic;
            isPlayerInDungeon = true;
            musicSpeaker.PlayDelayed(1);
            musicSpeaker.loop = true;
        }

        private void StartDungeonMusic(OnPortalEntered data)
        {
            isPlayerInDungeon = true;
            musicSpeaker.clip = dungeonMusic;
            musicSpeaker.PlayDelayed(1);
            musicSpeaker.loop = true;
        }
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;

                EventManager.Instance.RemoveListener<OnDungeonLeft>(StartFightMusic);
                EventManager.Instance.RemoveListener<OnPortalEntered>(StartDungeonMusic);
                EventManager.Instance.RemoveListener<OnMenuOpened>(StartMenuMusic);
                EventManager.Instance.RemoveListener<OnMenuClosed>(ResumeMusicAfterMenuClosed);
                EventManager.Instance.RemoveListener<OnGameLoaded>(StartDungeonMusic);
            }
        }
    }
}

