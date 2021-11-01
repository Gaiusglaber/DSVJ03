using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private KeyCode keyToTurnOnLantern = KeyCode.Escape;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 180;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _slideSpeed;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _slopeForce;
    [SerializeField] private Transform _raycast;
    private float _groundRayDistance = 1;
    private RaycastHit _slopeHit;

    private int _jumpCounter;
    private Vector3 _moveDirection;
    private Vector3 _rotation;
    public Vector3 _velocity;
    private CharacterController _controller;
    private Animator _animator;

    public event Action OnPlayerDie;
    public event Action OnPlayerCompletedLevel;
    public event Action OnTurnOnLantern;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
        if (Input.GetKeyDown(keyToTurnOnLantern))
        {
            OnTurnOnLantern?.Invoke();
        }
        if (OnSteepSlope()) SteepSlopeMovement();
        if (transform.position.y <= -36)
        {
            OnPlayerDie?.Invoke();
        }
    }

    private void Move()
    {
        _animator.SetFloat("Speed", 0);
        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            _jumpCounter = 0;
        }

        float moveZ = Input.GetAxis("Vertical");
        _rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * _rotationSpeed * Time.deltaTime, 0);
        _moveDirection = new Vector3(0, 0, moveZ);
        _moveDirection = transform.TransformDirection(_moveDirection);

        if (_moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
            _animator.SetFloat("Speed", 1);
        }

        if (_isGrounded)
        {
            if (_moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
                _animator.SetFloat("Speed", 1);
            }
            else if (_moveDirection == Vector3.zero)
            {
                Idle();
            }
        }
        if (_isGrounded || _jumpCounter < 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetBool("IsJumpStart", true);
                _jumpCounter++;
            }
        }

        if (_animator.GetBool("IsJumpStart") && _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpStart"))
        {
            _animator.SetBool("IsJumpStart", false);
            Jump();
        }


        _moveDirection *= _moveSpeed;

        if (!_isGrounded && Physics.Raycast(_raycast.position, _raycast.forward, 1, _groundMask))
        {
            _moveSpeed = 0;
        }
        else
        {
            _controller.Move(_moveDirection * Time.deltaTime);
        }

        transform.Rotate(_rotation);

        _velocity.y += _gravity * Time.deltaTime; //solo toca y

        if (_velocity.y <= 0)
        {
            _animator.SetBool("IsJumpDown", true);
        }
        else if (_isGrounded && _animator.GetBool("IsJumpDown")) 
        {
            _animator.SetBool("IsLanding",true);
            _animator.SetBool("IsJumpDown", false);
        }

        _controller.Move(_velocity * Time.deltaTime); //solo toca y
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
        _animator.SetBool("IsSliding", false);
        return false;
    }
    private void SteepSlopeMovement()
    {
        _animator.SetBool("IsSliding", true);
        Vector3 slopedirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
        float slideSpeed = _moveSpeed + _slideSpeed;

        _moveDirection = slopedirection * -slideSpeed;
        _moveDirection.y = _moveDirection.y - _slopeHit.point.y;

        _controller.Move(_moveDirection * Time.deltaTime);
        _controller.Move(Vector3.down * _controller.height / 2 * _slopeForce * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("LevelCompletion"))
        {
            OnPlayerCompletedLevel?.Invoke();
        }
    }
}