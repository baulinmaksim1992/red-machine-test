using Camera;
using UnityEngine;


namespace Connection
{
    public class ColorConnector : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float maxLength;

        [Header("Components")] 
        [SerializeField] private Transform lineTransform;
        [SerializeField] private SpriteRenderer lineSpriteRenderer;

        public Color Color => lineSpriteRenderer.color;

        public bool CanFinishConnecting { get; private set; }

        private Vector2 _startPoint;

        private float _lineWidth;
        private Vector2 _linePosition;
        private Vector2 _lineDirection;

        private bool _isConnecting;


        private void Update()
        {
            if (!_isConnecting)
                return;

            UpdateGeometry();
        }

        private void FixedUpdate()
        {
            if (!_isConnecting)
                return;

            UpdateParameters(CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        private void UpdateGeometry()
        {
            lineTransform.position = _linePosition;
            lineTransform.right = _lineDirection;
            lineSpriteRenderer.size = new Vector2(_lineWidth, 1.0f);
        }

        private void UpdateParameters(Vector2 secondPoint)
        {
            var distance = Vector2.Distance(secondPoint, _startPoint);

            CanFinishConnecting = maxLength > distance;
                
            _lineWidth = Mathf.Min(maxLength, distance);
            _lineDirection = (secondPoint - _startPoint).normalized;
            _linePosition = _startPoint + _lineWidth * _lineDirection / 2.0f;
        }

        public void StartConnecting(Vector2 startPoint, Color color)
        {
            _startPoint = startPoint;
            _isConnecting = true;

            lineSpriteRenderer.color = color;
            UpdateParameters(CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        public void FinishConnecting(Vector2 finishPosition)
        {
            _isConnecting = false;

            UpdateParameters(finishPosition);
            UpdateGeometry();
        }
    }
}