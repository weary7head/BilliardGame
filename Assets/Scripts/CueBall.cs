using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CueBall : MonoBehaviour
{
    [SerializeField] private LayerMask _ballMask;
    [SerializeField] private LayerMask _tableMask;
    [SerializeField] private float _rotateSpeed = 0.1f;
    [SerializeField] private GameObject _lineObject;

    private float _deltaFingerPosition;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private LineRenderer _cueLineRenderer;
    private LineRenderer _lineRenderer;
    private RaycastHit _hitInfo;
    private LayerMask _layerMask;
    private float _hitStrength;
    private float _trajectoryLength = 2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cueLineRenderer = Instantiate(_lineObject).GetComponent<LineRenderer>();
        _lineRenderer = Instantiate(_lineObject).GetComponent<LineRenderer>();
        _layerMask = _ballMask | _tableMask;
        _hitStrength = 0;
        _transform = transform;
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
    }

    private void Update()
    {
        Rotate();

        if (Physics.Raycast(_transform.position, _transform.forward, out _hitInfo, Mathf.Infinity, _layerMask))
        {
            Vector3 vectorReflected = Vector3.Reflect(_hitInfo.point - _transform.position, _hitInfo.normal);
            vectorReflected.y = 3;

            DrawTrajectory(_cueLineRenderer, _transform.position, _hitInfo.point, vectorReflected);

            if (_hitInfo.collider.gameObject.TryGetComponent(out Ball ball))
            {
                Vector3 direction = Vector3.Normalize(ball.transform.position - _hitInfo.point) * _trajectoryLength;
                DrawTrajectory(_lineRenderer, _hitInfo.point, ball.transform.position + direction);
            }
        }
    }

    public void Hit()
    {
        _rigidbody.AddForce(_transform.forward * _hitStrength, ForceMode.Impulse);
    }

    public void ChangeHitStrength(float value)
    {
        _hitStrength = value;
    }

    public void ResetPosition()
    {
        _transform.position = new Vector3(5, 2, 2);
    }    

    private void Rotate()
    {
    #if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _deltaFingerPosition = Vector3.Distance(_transform.position, touch.position);
            if (touch.phase == TouchPhase.Moved)
            {
                Quaternion rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * _rotateSpeed * Time.deltaTime, 0f);
                transform.rotation = rotationY * transform.rotation;
            }
        }
    #endif

    #if UNITY_EDITOR_WIN
        if (Input.GetMouseButton(1))
        {
            Quaternion rotationY = Quaternion.Euler(0f, -Input.mousePosition.x * _rotateSpeed * Time.deltaTime, 0f);
            transform.rotation = rotationY * transform.rotation;
        }
    #endif
    }

    private void DrawTrajectory(LineRenderer lineRenderer,params Vector3[] positions)
    {
        lineRenderer.positionCount = positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
