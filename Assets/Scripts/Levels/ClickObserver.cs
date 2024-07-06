using System;
using Connection;
using Events;
using Player.ActionHandlers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Levels
{
    public class ClickObserver : MonoBehaviour
    {
        [SerializeField] private ColorConnectionManager colorConnectionManager;

        private ClickHandler _clickHandler;

        private void Awake()
        {
            _clickHandler = ClickHandler.Instance;

            _clickHandler.PointerDownEvent += OnPointerDown;
            _clickHandler.PointerUpEvent += OnPointerUp;
            _clickHandler.ScrollEvent += OnScroll;
        }

        private void OnDestroy()
        {
            _clickHandler.PointerDownEvent -= OnPointerDown;
            _clickHandler.PointerUpEvent -= OnPointerUp;
            _clickHandler.ScrollEvent -= OnScroll;
        }

        private void OnPointerDown(Vector3 position)
        {
            colorConnectionManager.TryGetColorNodeInPosition(position, out var node);

            if (node != null)
            {
                EventsController.Fire(new EventModels.Game.NodeTapped());
            }
            else if(node == null && !EventSystem.current.IsPointerOverGameObject())
            {
                EventsController.Fire(new EventModels.Game.CameraMoveStarted());
            }
        }

        private void OnPointerUp(Vector3 position)
        {
            EventsController.Fire(new EventModels.Game.PlayerFingerRemoved());
        }

        private void OnScroll(float scroll)
        {
            EventsController.Fire(new EventModels.Game.ScrollStarted());
        }
    }
}