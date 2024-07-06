using Events;
using Player;
using Player.ActionHandlers;
using UnityEngine;
using Utils.Singleton;


namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        public UnityEngine.Camera MainCamera => mainCamera;

        [Header("Camera Movement Settings")]
        [Tooltip("Optimal between 14 and 18")]
        [SerializeField, Range(4f, 20f)] private float _horizontalMoveSpeed;

        [Tooltip("Optimal between 12 and 16")]
        [SerializeField, Range(4f, 20f)] private float _verticalMoveSpeed;

        [Space(20)]

        [Tooltip("Optimal between 4 and 8")]
        [SerializeField, Range(1f, 10f)] private float _zoomSensetive;

        [Space(20)]

        [SerializeField, Range(10f, 20f)] private float _maxCameraDistance;

        

        private float _minCameraDistance = 5f;
        private float _zoomSensitiveAmplifire = 200f;
        private Vector3 _lastMousePosition;
        private bool _moveMode;
        private ClickHandler _clickHandler;

        protected override void Awake()
        {
            base.Awake();

            _clickHandler = ClickHandler.Instance;
            _clickHandler.SubscribeDragEventHandlers(OnMoveStar, OnMoveEnd);
            EventsController.Subscribe<EventModels.Game.ScrollStarted>(this, OnScroll);
        }

        private void Update()
        {
            if (_moveMode)
            {
                Vector3 deltaMousePosition = Input.mousePosition - _lastMousePosition;
                Vector3 moveDirection = new Vector3(deltaMousePosition.x, deltaMousePosition.y, 0).normalized * -1;

                float moveAmplifier = 0.2f * mainCamera.orthographicSize;
                //var moveAmplifier = 1f;

                transform.position += new Vector3(moveDirection.x * _horizontalMoveSpeed * moveAmplifier , moveDirection.y * _verticalMoveSpeed * moveAmplifier, 0) * Time.deltaTime;

                _lastMousePosition = Input.mousePosition;
            }
        }

        private void OnDestroy()
        {
            _clickHandler.UnsubscribeDragEventHandlers(OnMoveStar, OnMoveEnd);
            EventsController.Unsubscribe<EventModels.Game.ScrollStarted>(OnScroll);
        }

        public void OnMoveStar(Vector3 position)
        {
            if (PlayerController.PlayerState != PlayerState.CameraMoving)
            {
                return;
            }

            _lastMousePosition = position;
            _moveMode = true;
        }

        public void OnMoveEnd(Vector3 position)
        {
            _moveMode = false;
        }

        public void OnScroll(EventModels.Game.ScrollStarted e)
        {
            if (PlayerController.PlayerState == PlayerState.CameraMoving)
            {
                return;
            }

            if (PlayerController.PlayerState == PlayerState.Connecting)
            {
                return;
            }

            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0.0f)
            {
                mainCamera.orthographicSize -= scroll * _zoomSensetive * _zoomSensitiveAmplifire * Time.deltaTime;
                mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, _minCameraDistance, _maxCameraDistance); // Ограничиваем минимальное и максимальное значение
            }
        }
    }
}