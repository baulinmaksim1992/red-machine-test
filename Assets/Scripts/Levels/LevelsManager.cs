using Events;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviour
    {
        [SerializeField] bool _testLevelFirst;
        private const string LevelNamePattern = "Level{0}";

        private int _currentLevelIndex;

        private void Start()
        {
            if (_testLevelFirst)
            {
                ScenesChanger.GotoScene("TestLevel");
            }
            else
            {
                ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
            }

            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            if (_testLevelFirst)
            {
                ScenesChanger.GotoScene("TestLevel");
            }
            else
            {
                _currentLevelIndex += 1;
                ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
            }
        }
    }
}