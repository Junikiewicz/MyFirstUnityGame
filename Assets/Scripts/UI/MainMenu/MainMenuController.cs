using MyRPGGame.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.UI
{
    class MainMenuController:MonoBehaviour
    {
        private static MainMenuController _instance;
        bool gameRunning = false;
        Stack<MenuItem> menuItems = new Stack<MenuItem>();
        public MenuItem defaulttMenuItem;
        public static MainMenuController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<MainMenuController>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<OnNewGame>(GameStarted);
                    EventManager.Instance.AddListener<OnGameLoaded>(GameLoaded);
                    EventManager.Instance.AddListener<OnPlayerKilled>(GameEnded);
                    _instance = this;
                }
                else
                {
                    enabled = false;
                    Debug.LogError(GetType() + " couldn't find EventManager.");
                }
            }
        }
        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (menuItems.Count == 0)
                {
                    OpenMainMenu();
                }
                else
                {
                    if (menuItems.Count == 1)
                    {
                        if (gameRunning)
                            CloseMainMenu();
                    }
                    else
                    {
                        HideTopElement();
                    }
                }
            }
        }
        public void CloseMainMenu()
        {
            EventManager.Instance.TriggerEvent(new OnMenuClosed());
            int amountOfItems = menuItems.Count;
            for(int i=0;i<amountOfItems;i++)
            {
                HideTopElement();
            }
            gameObject.GetComponent<Image>().enabled = false;
            EventManager.Instance.TriggerEvent(new OnPauseEnd());
        }
        public void OpenMainMenu()
        {
            EventManager.Instance.TriggerEvent(new OnMenuOpened());
            gameObject.GetComponent<Image>().enabled = true;
            if(defaulttMenuItem!=null)
            {
                AddNewElementOnTop(defaulttMenuItem);
            }
            EventManager.Instance.TriggerEvent(new OnPauseStart());
        }
        public void HideTopElement()
        {
                menuItems.Pop().Hide();
           
            if (menuItems.Count > 0)
            {
                menuItems.Peek().Show();
            }
        }
        public void AddNewElementOnTop(MenuItem newItem)
        {
            if(menuItems.Count>0)
            {
                menuItems.Peek().Hide();
            }
            newItem.Show();
            menuItems.Push(newItem);
        }

        private void GameStarted(OnNewGame date)
        {
            gameRunning = true;
        }

        private void GameLoaded(OnGameLoaded date)
        {
            gameRunning = true;
        }

        private void GameEnded(OnPlayerKilled data)
        {
            gameRunning = false;
        }

        private void OnDestroy()
        {
            if(EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnNewGame>(GameStarted);
                EventManager.Instance.RemoveListener<OnGameLoaded>(GameLoaded);
                EventManager.Instance.RemoveListener<OnPlayerKilled>(GameEnded);
            }
        }
    }
}
