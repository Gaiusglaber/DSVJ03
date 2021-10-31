using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    private float _currentDistance;
    [SerializeField] private float minDistance = 1;              
    [SerializeField] private float maxDistance = 2;
    [SerializeField] private float DistanceUp = -2;                    
    [SerializeField] private float smooth = 4.0f;                
    [SerializeField] private float rotateAround = 70f;
    [SerializeField] private Transform target;                   
    [SerializeField] private LayerMask targetLayer;
    private RaycastHit _hit;
    private float _cameraHeight = 55f;
    private float _cameraPan = 0f;
    private float _camRotateSpeed = 180f;
    private float _horizontalAxis;
    private float _verticalAxis;
    private Vector3 _camPosition;
    private Vector3 _camMask;
    private Vector3 _followMask;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    void Start()
    {
        rotateAround = target.eulerAngles.y - 45f;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    dragOrigin = Input.mousePosition;
        //    return;
        //}

        //if (!Input.GetMouseButton(0)) return;

        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        //transform.Translate(move, Space.World);
    }
    void LateUpdate()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");

        Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
        Quaternion rotation = Quaternion.Euler(_cameraHeight, rotateAround, _cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;
       
        _camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * _currentDistance;
        _camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * _currentDistance;

        occludeRay(ref targetOffset);
        smoothCamMethod();

        transform.LookAt(target);

        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }

        rotateAround += _horizontalAxis * _camRotateSpeed * Time.deltaTime;
        _currentDistance = Mathf.Clamp(_currentDistance += _verticalAxis, minDistance, maxDistance);
    }
    void smoothCamMethod()
    {
        smooth = 4f;
        transform.position = Vector3.Lerp(transform.position, _camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        RaycastHit wallHit = new RaycastHit();

        if (!Physics.Linecast(targetFollow, _camMask, out wallHit, targetLayer)) return;
        smooth = 10f;
        _camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, _camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
    }
}