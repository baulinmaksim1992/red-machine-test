using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class EventsController
    {
        private static EventsController _instance;

        private readonly Dictionary<Type, EventHandleCore> _handlers = new Dictionary<Type, EventHandleCore>(100);

        public static EventsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventsController();
                    _instance.AddHelper();
                }

                return _instance;
            }
        }

        public Dictionary<Type, EventHandleCore> Handlers => _handlers;

        public static void Subscribe<T>(object watcher, Action<T> action) where T : struct, IEvent
        {
            Instance.Sub(watcher, action);
        }

        public static void Unsubscribe<T>(Action<T> action) where T : struct, IEvent
        {
            if (_instance != null)
            {
                Instance.Unsub(action);
            }
        }

        public static void Fire<T>(T args) where T : struct, IEvent
        {
            Instance.FireEvent(args);
        }

        public static bool HasWatchers<T>() where T : struct, IEvent
        {
            return Instance.HasWatchersDirect<T>();
        }

        private void Sub<T>(object watcher, Action<T> action)
        {
            EventHandleCore value = null;
            if (!_handlers.TryGetValue(typeof(T), out value))
            {
                value = new EventHandle<T>();
                _handlers.Add(typeof(T), value);
            }

            (value as EventHandle<T>).Subscribe(watcher, action);
        }

        private void Unsub<T>(Action<T> action)
        {
            EventHandleCore value = null;
            if (_handlers.TryGetValue(typeof(T), out value))
            {
                (value as EventHandle<T>).Unsubscribe(action);
            }
        }

        private void FireEvent<T>(T args)
        {
            EventHandleCore value = null;
            if (!_handlers.TryGetValue(typeof(T), out value))
            {
                value = new EventHandle<T>();
                _handlers.Add(typeof(T), value);
            }

            (value as EventHandle<T>).Fire(args);
        }

        private bool HasWatchersDirect<T>() where T : struct
        {
            EventHandleCore value = null;
            if (_handlers.TryGetValue(typeof(T), out value))
            {
                return value.Watchers.Count > 0;
            }

            return false;
        }

        private void AddHelper()
        {
            new GameObject("[EventHelper]").AddComponent<EventsControllerSetup>();
        }

        public void CheckHandlersOnLoad()
        {
            Dictionary<Type, EventHandleCore>.Enumerator enumerator = _handlers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.FixWatchers();
            }
        }

        public void CleanUp()
        {
            Dictionary<Type, EventHandleCore>.Enumerator enumerator = _handlers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.CleanUp();
            }
        }
    }
}