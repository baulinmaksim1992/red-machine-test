using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.MonoBehaviourUtils;

namespace Utils.RealTimeTicker
{
    public class RealTimeTicker : GameMonoBehaviour
    {
        private static readonly HashSet<TickDelegate> Listeners = new();

        public delegate void TickDelegate(float deltaRealTime);

        public static bool IsPaused { get; set; }
        
        private static float _realtimeSinceStartup;
        

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (IsPaused)
                return;
            
            var realtimeSinceStartup = Time.realtimeSinceStartup;
            var deltaRealTime = realtimeSinceStartup - _realtimeSinceStartup;
            _realtimeSinceStartup = realtimeSinceStartup;

            Tick(deltaRealTime);
        }

        private static void Tick(float deltaRealTime)
        {
            foreach (var tickListener in Listeners.ToArray())
                tickListener?.Invoke(deltaRealTime);
        }

        public static void AddListener(TickDelegate listener)
        {
            Listeners.Add(listener);
        }

        public static void RemoveListener(TickDelegate listener)
        {
            Listeners.Remove(listener);
        }
    }
}