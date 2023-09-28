using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System;
using UnityEngine.Scripting;

[Serializable]
public class Sound
{
    public AudioClip clip;
    public bool loop;
    [Range(0f, 1f)]
    public float volume;
    [Range(1f, 3f)]
    public float pitch;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 _inputs;
    [SerializeField] PlayerInputs _input;
    public bool _activeOrNot;
    [SerializeField] bool _inputJump;
    [SerializeField] Rigidbody2D _rb;

    [Header("Movements")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _acceleration;

    [Header("Groundcheck")]
    [SerializeField] float _groundOffset;
    [SerializeField] float _groundRadius;
    [SerializeField] LayerMask _GroundLayer;
    [SerializeField] [Tooltip("Collider list of all plateforms of the scene")] Collider2D[] _collidersGround;
    [SerializeField] [Tooltip("Touching ground or not")] bool _isGrounded;

    [Header("Jump")]
    [SerializeField] [Tooltip("Time minimum between jumps")] float _timerMinBetweenJump;
    [SerializeField] float _jumpForce;
    [SerializeField] [Tooltip("Fall speed")] float _velocityFallMin;
    [SerializeField] [Tooltip("Gravity when the player goes up and press jump")] float _gravityUpJump;
    [SerializeField] [Tooltip("Gravity otherwise")] float _gravity;
    [SerializeField] float _jumpInputTimer = 0.1f;
    [SerializeField] float _timerNoJump;
    [SerializeField] float _timerSinceJumpPressed;
    [SerializeField] float _TimeSinceGrounded;
    [SerializeField] float _TimeSinceNotGrounded;

    [Header("Teleportation")]
    public int NumberTeleport = 1;
    public Teleport _teleport;
    public bool teleportationClick;
    public Vector2 _cursor;
    public bool IsGamepad;

    [Header("Anim")]
    [SerializeField] GameObject PlayerMesh;

    [Header("Sounds")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Sound _walkSound;
    [SerializeField] Sound _jumpSound;
    [SerializeField] Sound _hitGround;

    [Header("Idk")]
    [SerializeField] float _coyoteTime;
    [SerializeField] float _slopeDetectOffset;
    [SerializeField] bool _isOnSlope;
    [SerializeField] Collider2D _collider;
    [SerializeField] PhysicsMaterial2D _physicsFriction;
    [SerializeField] PhysicsMaterial2D _physicsNoFriction;
    Vector3 _offsetCollisionBox;
    Vector3 _offsetToReplace;
    Vector2 _collisionBox;

    RaycastHit2D[] _hitResults = new RaycastHit2D[2];
    float[] directions = new float[] { 1, -1 };
    public ScreenShake _shake;
    public ParticleSystem _particules;




    private void Awake()
    {
        _input = new PlayerInputs();

        UnityEngine.Cursor.visible = false;
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += GetMoveInputs;
        _input.Player.CursorGamepad.performed += GetCursorInputsGamepad;
        _input.Player.CursorMouse.performed += GetCursorInputsMouse;

        _input.Player.Jump.performed += GetJumpInputs;
        _input.Player.Teleportation.performed += GetShootInputs;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Move.performed -= GetMoveInputs;
        _input.Player.CursorGamepad.performed -= GetCursorInputsGamepad;
        _input.Player.CursorMouse.performed -= GetCursorInputsMouse;

        _input.Player.Jump.performed -= GetJumpInputs;
        _input.Player.Teleportation.performed -= GetShootInputs;
    }

    #region inputs
    void GetMoveInputs(InputAction.CallbackContext move)
    {
        if (_activeOrNot)
            _inputs = move.ReadValue<Vector2>();
    }
    
    void GetCursorInputsGamepad(InputAction.CallbackContext cursor)
    {
        if (_activeOrNot)
            IsGamepad = true;
    }
    
    void GetCursorInputsMouse(InputAction.CallbackContext cursor)
    {
        if (_activeOrNot)
            IsGamepad = false;
    }

    void GetJumpInputs(InputAction.CallbackContext jump)
    {
        if (_activeOrNot)
        {
            _inputJump = true;
            _timerSinceJumpPressed = 0;
        }
    }

    void GetShootInputs(InputAction.CallbackContext tp)
    {
        if (_activeOrNot)
            teleportationClick = true;
    }

    void HandleInputs()
    {
        if (_activeOrNot)
        {
            if (IsGamepad && _input.Player.CursorGamepad.ReadValue<Vector2>() != Vector2.zero)
            {
                _cursor = _input.Player.CursorGamepad.ReadValue<Vector2>();
            }
            else if (!IsGamepad)
            {
                _cursor = Camera.main.ScreenToWorldPoint(_input.Player.CursorMouse.ReadValue<Vector2>());
            }

            if (teleportationClick)
            {
                teleportationClick = false;
            }
        }
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        AnimPlayer();
        HandleMovements();
        HandleGrounded();
        HandleJump();
        HandleSlope();
        HandleCorners();
    }
    #endregion

    #region move ground n jump
    void HandleMovements()
    {
        var velocity = _rb.velocity;
        Vector2 wantedVelocity = new Vector2(_inputs.x * _walkSpeed, velocity.y);
        _rb.velocity = Vector2.MoveTowards(velocity, wantedVelocity, _acceleration * Time.deltaTime);

        if (_rb.velocity.x != 0 && _isGrounded)
        {
            PlaySound(_walkSound, _audioSource);
        }
    }

    Vector2 point;

    void HandleGrounded()
    {
        _TimeSinceGrounded += Time.deltaTime;

        point = transform.position + Vector3.up * _groundOffset;
        bool currentGrounded = Physics2D.OverlapCircleNonAlloc(point, _groundRadius, _collidersGround, _GroundLayer) > 0;

        if (currentGrounded == false && _isGrounded)
        {
            _TimeSinceGrounded = 0;
        }

        if (_isGrounded && NumberTeleport <= 0)
        {
            NumberTeleport = 1;
        }

        _isGrounded = currentGrounded;

        if (_isGrounded) 
        {
            _TimeSinceNotGrounded = 0;
        }
        else
        {
            _TimeSinceNotGrounded += Time.deltaTime;
        }

        bool currentTouching = Physics2D.OverlapCircleNonAlloc(new Vector2(point.x, point.y - 0.2f), _groundRadius, _collidersGround, _GroundLayer) > 0;
        if (currentTouching && _rb.velocity.y < 0)
        {
            if (_TimeSinceNotGrounded > 3)
            {
                _shake.ShakeCamera();
            }

            PlaySound(_hitGround, _audioSource);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point, _groundRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(point.x, point.y - 0.2f), _groundRadius);
        Gizmos.color = Color.white;
    }


    void HandleJump()
    {
        _timerNoJump -= Time.deltaTime;
        _timerSinceJumpPressed += Time.deltaTime;

        //Limite vitesse chute
        if (_rb.velocity.y < _velocityFallMin)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _velocityFallMin);
        }

        if (_isGrounded == false)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _gravity;
            }
            else
            {
                _rb.gravityScale = _inputJump ? _gravityUpJump : _gravity;
            }
        }
        else
        {
            _rb.gravityScale = _gravity;
        }

        if (_inputJump && (_rb.velocity.y <= 0 || _isOnSlope) && (_isGrounded || _TimeSinceGrounded < _coyoteTime) && _timerNoJump <= 0 && _timerSinceJumpPressed < _jumpInputTimer)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _timerNoJump = _timerMinBetweenJump;

            PlaySound(_jumpSound, _audioSource);
            _particules.Play();
        }


        if (!_input.Player.Jump.IsPressed())
        {
            _inputJump = false;
        }
    }
    #endregion

    #region slope n corner
    void HandleSlope()
    {
        Vector3 origin = transform.position + Vector3.up * _groundOffset;
        bool slopeRight = Physics2D.RaycastNonAlloc(origin, Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;
        bool slopeLeft = Physics2D.RaycastNonAlloc(origin, -Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;

        _isOnSlope = (slopeRight || slopeLeft) && (slopeRight == false || slopeLeft == false);

        if (Mathf.Abs(_inputs.x) < 0.1f && (slopeLeft || slopeRight))
        {
            _collider.sharedMaterial = _physicsFriction;
        }
        else
        {
            _collider.sharedMaterial = _physicsNoFriction;
        }
    }

    void HandleCorners()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            float dir = directions[i];

            if (Mathf.Abs(_inputs.x) > 0.1f && Mathf.Abs(Mathf.Sign(dir) - Mathf.Sign(_inputs.x)) < 0.001f && _isGrounded == false && _isOnSlope == false)
            {
                Vector3 position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x, _offsetCollisionBox.y, 0);
                int result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero, _hitResults, 0, _GroundLayer);

                if (result > 0)
                {
                    position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x, _offsetCollisionBox.y + _offsetToReplace.y, 0);
                    result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero, _hitResults, 0, _GroundLayer);

                    if (result == 0)
                    {
                        Debug.Log("replace");
                        transform.position += new Vector3(dir * _offsetToReplace.x, _offsetToReplace.y);

                        if (_rb.velocity.y < 0)
                        {
                            _rb.velocity = new Vector2(_rb.velocity.x, 0);
                        }
                    }
                }
            }
        }
    }
#endregion

    void AnimPlayer()
    {
        if (_inputs.x != 0)
        {
            int side = 0;

            if (_inputs.x < 0)
                side = -1;
            else
                side = 1;

            if (PlayerMesh.transform.localScale.x != side)
            {
                _particules.Play();
            }

            PlayerMesh.transform.localScale = new Vector3(side, PlayerMesh.transform.localScale.y, PlayerMesh.transform.localScale.z);
        }
    }

    public void PlaySound(Sound _sound, AudioSource _audioSource)
    {
        if (_audioSource.isPlaying && _audioSource.clip == _sound.clip)
            return;

        _audioSource.Stop();
        _audioSource.clip = _sound.clip;
        _audioSource.loop = _sound.loop;
        _audioSource.volume = _sound.volume;
        _audioSource.pitch = _sound.pitch;
        _audioSource.Play();

        //Debug.Log($@"clip : {_audioSource.clip} : {_sound.clip}
//loop : {_audioSource.loop} : {_sound.loop}
//volume : {_audioSource.volume} : {_sound.volume}
//pitch : {_audioSource.pitch} : {_sound.pitch}");
    }
}
