using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _slideSpeed;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    private float _groundRayDistance = 1;
    private RaycastHit _slopeHit;

    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        if (OnSteepSlope()) SteepSlopeMovement();
    }

    private void Move()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckDistance,_groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        _moveDirection = new Vector3(moveX, 0, moveZ);

        if (_isGrounded)
        {
            if (_moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (_moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (_moveDirection == Vector3.zero)
            {
                Idle();
            }

            _moveDirection *= _moveSpeed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        _controller.Move(_moveDirection * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Idle()
    {

    }

    private void Walk()
    {
        _moveSpeed = _walkSpeed;
    }

    private void Run()
    {
        _moveSpeed = _runSpeed;
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    private bool OnSteepSlope()
    {
        if (!_isGrounded) return false;

        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit,
            (_controller.height / 2) + _groundRayDistance))
        {
            float _slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
            if (_slopeAngle > _controller.slopeLimit) return true;
        }

        return false;
    }
    private void SteepSlopeMovement()
    {
        Vector3 slopedirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
        float slideSpeed = _moveSpeed + _slideSpeed;

        _moveDirection = slopedirection * -slideSpeed;
        _moveDirection.y = _moveDirection.y - _slopeHit.point.y;

        _controller.Move(_moveDirection * Time.deltaTime);
    }
}