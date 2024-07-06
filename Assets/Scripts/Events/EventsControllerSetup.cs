using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    public class EventsControllerSetup : MonoBehaviour
    {
        [Serializable]
        public class EventData
        {
            public string Name = string.Empty;

            public Type Type;

            public List<MonoBehaviour> MonoWatchers = new List<MonoBehaviour>(100);

            public List<string> OtherWatchers = new List<string>(100);

            public EventData(Type type)
            {
                Type = type;
                Name = type.ToString();
            }
        }

        public List<EventData> Events = new List<EventData>(100);

        private Dictionary<Type, string> _typeCache = new Dictionary<Type, string>();

        private float _cleanupTimer;

        public bool AutoFill => false;

        private void Awake()
        {
            DontDestroyOnLoad(base.gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EventsController.Instance.CheckHandlersOnLoad();
        }

        private void SubscribeToLog<T>() where T : struct, IEvent
        {
            EventsController.Subscribe<T>(this, OnLog);
        }

        private void OnLog<T>(T ev) where T : struct, IEvent
        {
            Debug.LogFormat("Event: {0}", typeof(T));
        }

        private void Update()
        {
            TryCleanUp();
            if (AutoFill)
            {
                Fill();
            }
        }

        [ContextMenu("CheckEventHandlers")]
        public void CheckEventHandlers()
        {
            foreach (KeyValuePair<Type, EventHandleCore> handler in EventsController.Instance.Handlers)
            {
                if (handler.Value.Watchers.Count > 0)
                {
                    Debug.Log(handler.Key);
                    foreach (object watcher in handler.Value.Watchers)
                    {
                        Debug.Log(string.Concat(handler.Key, " => ", watcher.GetType().ToString()));
                    }
                }
            }
        }

        [ContextMenu("ClearEventHandlers")]
        public void ClearEventHandlers()
        {
            EventsController.Instance.Handlers.Clear();
        }

        private void TryCleanUp()
        {
            if (_cleanupTimer > 10f)
            {
                EventsController.Instance.CleanUp();
                _cleanupTimer = 0f;
            }
            else
            {
                _cleanupTimer += Time.deltaTime;
            }
        }

        [ContextMenu("Fill")]
        public void Fill()
        {
            Dictionary<Type, EventHandleCore>.Enumerator
                enumerator = EventsController.Instance.Handlers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, EventHandleCore> current = enumerator.Current;
                EventData eventData = GetEventData(current.Key);
                if (eventData == null)
                {
                    eventData = new EventData(current.Key);
                    Events.Add(eventData);
                }

                FillEvent(current.Value, eventData);
            }
        }

        private void FillEvent(EventHandleCore handler, EventData data)
        {
            data.MonoWatchers.Clear();
            data.OtherWatchers.Clear();
            List<object>.Enumerator enumerator = handler.Watchers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if (current is MonoBehaviour)
                {
                    data.MonoWatchers.Add(current as MonoBehaviour);
                }
                else
                {
                    data.OtherWatchers.Add((current != null) ? GetTypeNameFromCache(current.GetType()) : "null");
                }
            }
        }

        private EventData GetEventData(Type type)
        {
            List<EventData>.Enumerator enumerator = Events.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Type == type)
                {
                    return enumerator.Current;
                }
            }

            return null;
        }

        private string GetTypeNameFromCache(Type type)
        {
            string value = string.Empty;
            if (!_typeCache.TryGetValue(type, out value))
            {
                value = type.ToString();
                _typeCache.Add(type, value);
            }

            return value;
        }
    }
}