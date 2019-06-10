using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPGGame.Events
{
    public sealed class EventManager : ScriptableObject
    {
        private static EventManager m_instance = null;

        public delegate void EventDelegate<T>(T e) where T : IGameEvent;
        private delegate void EventDelegate(IGameEvent e);

        private Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();
        private Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();
        public static EventManager Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = FindObjectOfType<EventManager>();
                }
                if (!m_instance)
                {
                    m_instance = CreateInstance<EventManager>();
                }
                return m_instance;
            }
        }
        private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : IGameEvent
        {
            if (delegateLookup.ContainsKey(del))
            {
                return null;
            }

            EventDelegate internalDelegate = (e) => del((T)e);
            delegateLookup[del] = internalDelegate;

            if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
            {
                delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                delegates[typeof(T)] = internalDelegate;
            }

            return internalDelegate;
        }

        public void AddListener<T>(EventDelegate<T> del) where T : IGameEvent
        {
            AddDelegate(del);
        }
        public void RemoveListener<T>(EventDelegate<T> del) where T : IGameEvent
        {
            if (delegateLookup.TryGetValue(del, out EventDelegate internalDelegate))
            {
                if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegates.Remove(typeof(T));
                    }
                    else
                    {
                        delegates[typeof(T)] = tempDel;
                    }
                }
                delegateLookup.Remove(del);
            }
        }
        public void TriggerEvent(IGameEvent e)
        {
            if (delegates.TryGetValue(e.GetType(), out EventDelegate del))
            {
                del.Invoke(e);
            }
            else
            {
                Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
            }
        }
        public void RemoveAll()
        {
            delegates.Clear();
            delegateLookup.Clear();
        }
        public void OnDestroy()
        {
            RemoveAll();
            if(m_instance==this)
            {
                m_instance = null;
            }
        }
    }

}