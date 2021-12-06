using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private KeyCode keyToTurnOnLantern = KeyCode.Escape;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 180;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _slideSpeed;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _slopeForce;
    [SerializeField] private Transform _raycast;
    [SerializeField] private float TimeToDespawnCollectables = 0;
    [SerializeField] private NPC[] npcs = null;
    private float _groundRayDistance = 1;
    private RaycastHit _slopeHit;

    private int _jumpCounter;
    private bool pause = false;
    private Vector3 _moveDirection;
    private Vector3 _rotation;
    public Vector3 _velocity;
    private CharacterController _controller;
    private Animator _animator;
    private bool doubleJump = false;
    private bool canTalkToNPC = false;
    private bool triggerEvent = false;
    private GameObject talkingNpc = null;

    public event Action<float> OnTalkingToNpc;
    public event Action OnPause;
    public event Action OnUnpause;
    public event Action OnPlayerDie;
    public event Action OnPlayerCompletedLevel;
    public event Action OnTurnOnLantern;

    void Start()
    {
        foreach (var NPC in npcs)
        {
            NPC.OnGetCloseFromNPC += TalkToNPC;
        }
        CameraFollow.OnExitEvent += UnTriggerFlag;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void UnTriggerFlag()
    {
        triggerEvent = false;
    }
    private void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(keyToTurnOnLantern))
        {
            OnTurnOnLantern?.Invoke();
        }
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause)) && !pause && !triggerEvent)
        {
            OnPause?.Invoke();
            pause = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && canTalkToNPC && !triggerEvent)
        {
            OnTalkingToNpc?.Invoke(TimeToDespawnCollectables);
            triggerEvent = true;
        }
        if (OnSteepSlope())
        {
            SteepSlopeMovement();
        }

        Move();
    }
    void Update()
    {
        CheckKeyboardInput();

        Collider[] colliders = Physics.OverlapSphere(_raycast.position, 100, _groundMask);
        if (colliders.Length <= 0)
        {
            OnPlayerDie?.Invoke();
        }
    }
    private void TalkToNPC(bool canTalkToNPC, GameObject NPC)
    {
        this.canTalkToNPC = canTalkToNPC;
        if (canTalkToNPC)
        {
            talkingNpc = NPC;
        }
    }
    public void UnPause()
    {
        pause = false;
        OnUnpause?.Invoke();
    }
    private void Move()
    {
        _animator.SetFloat("Speed", 0);
        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
           // _jumpCounter = 0;
            doubleJump = false;
        }

        float moveZ = Input.GetAxis("Vertical");
        if (OnSteepSlope())
        {
            moveZ = 1;
        }
        _rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * _rotationSpeed * Time.deltaTime, 0);
        _moveDirection = new Vector3(0, 0, moveZ);
        _moveDirection = transform.TransformDirection(_moveDirection);

        if (_moveDirection != Vector3.zero)
        {
            Walk();
            _animator.SetFloat("Speed", 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jumpCounter <= 2) 
        {
            _jumpCounter++;
            AkSoundEngine.PostEvent("jump", gameObject);
            if (_jumpCounter == 2)
            {
                doubleJump = true;
            }
            StartCoroutine(Jump());
        }

        if (!_isGrounded && Physics.Raycast(_raycast.position, _raycast.forward, 1, _groundMask))
        {
            _moveSpeed = 0;
        }

        _moveDirection *= _moveSpeed;

        _controller.Move(_moveDirection * Time.deltaTime);

        transform.Rotate(_rotation);

        _velocity.y += _gravity * Time.deltaTime; //solo toca y
        _controller.Move(_velocity * Time.deltaTime); //solo toca y
    }

    private void Walk()
    {
        _moveSpeed = _walkSpeed;
    }

    IEnumerator Jump()
    {
        _animator.SetTrigger("OnJumpStart");
        yield return new WaitForSeconds(0.25f);
        _animator.ResetTrigger("OnJumpStart");

        _animator.SetTrigger("OnJumpUp");
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);

        while (_velocity.y >= -5f)
        {
            if (doubleJump)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
                doubleJump = false;
            }
            yield return new WaitForEndOfFrame();
        }

        _animator.SetTrigger("OnJumpDown");

        while (!_isGrounded)
        {
            yield return new WaitForEndOfFrame();
        }

        _animator.SetTrigger("OnJumpLand");
        _jumpCounter = 0;
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
        else if (other.transform.CompareTag("Enemy"))
        {
            OnPlayerDie?.Invoke();
        }
        else if (other.transform.CompareTag("Level1"))
        {
            GameManager.GetInstance().GameOver();
        }
    }

    public void ChangeScene()
    {
        GameManager.GetInstance().GoBackToHub();
    }
}