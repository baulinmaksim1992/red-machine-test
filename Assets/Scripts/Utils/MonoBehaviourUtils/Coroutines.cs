using System;
using System.Collections;
using UnityEngine;
using Utils.Singleton;

namespace Utils.MonoBehaviourUtils
{
    public class Coroutines : DontDestroyMonoBehaviourSingleton<Coroutines>
    {
        private static bool _isDestroyed;
        
        public static void Stop(IEnumerator coroutine)
        {
            if (_isDestroyed)
                return;
            
            Instance.StopCoroutine(coroutine);
        }

        public static void Stop(Coroutine coroutine)
        {
            if (_isDestroyed)
                return;
            
            Instance.StopCoroutine(coroutine);
        }

        public static Coroutine WaitForSeconds(float delay, Action callback)
        {
            if (_isDestroyed)
                return null;
            
            var routine = WaitCoroutine(new WaitForSeconds(delay), callback);
            return Instance.StartCoroutine(routine);
        }

        public static Coroutine WaitForSecondsRealtime(float delay, Action callback)
        {
            if (_isDestroyed)
                return null;
            
            var routine = WaitRoutine(new WaitForSecondsRealtime(delay), callback);
            return Instance.StartCoroutine(routine);
        }

        public static Coroutine RepeatEverySeconds(float repeatDelay, Action repeatCallback)
        {
            if (_isDestroyed)
                return null;
            
            var routine = RepeatRoutine(new WaitForSeconds(repeatDelay), repeatCallback);
            return Instance.StartCoroutine(routine);
        }

        public static Coroutine RepeatEverySecondsRealtime(float repeatDelay, Action repeatCallback)
        {
            if (_isDestroyed)
                return null;
            
            var routine = RepeatRoutine(new WaitForSecondsRealtime(repeatDelay), repeatCallback);
            return Instance.StartCoroutine(routine);
        }

        private static IEnumerator WaitCoroutine(YieldInstruction yieldInstruction, Action callback)
        {
            yield return yieldInstruction;

            callback?.Invoke();
        }

        private static IEnumerator WaitRoutine(IEnumerator enumerator, Action callback)
        {
            yield return enumerator;

            callback?.Invoke();
        }

        private static IEnumerator RepeatRoutine(YieldInstruction yieldInstruction, Action repeatCallback)
        {
            while (true)
            {
                yield return yieldInstruction;

                repeatCallback?.Invoke();
            }
        }

        private static IEnumerator RepeatRoutine(IEnumerator enumerator, Action repeatCallback)
        {
            while (true)
            {
                yield return enumerator;

                repeatCallback?.Invoke();
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();

            _isDestroyed = true;
        }
    }
}