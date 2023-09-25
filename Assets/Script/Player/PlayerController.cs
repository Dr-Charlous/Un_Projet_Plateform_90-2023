using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 _inputs;
    [SerializeField] bool _inputJump;
    [SerializeField] Rigidbody2D _rb;

    [Header("Movements")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _acceleration;

    [Header("Groundcheck")]
    [SerializeField] float _groundOffset;
    [SerializeField] float _groundRadius;
    [SerializeField] LayerMask _GroundLayer;
    [SerializeField] Collider2D[] _collidersGround;
    public string _tagGround;
    [SerializeField] bool _isGrounded;

    [Header("Jump")]
    [SerializeField] float _timerMinBetweenJump;
    [SerializeField] float _jumpForce;
    [SerializeField] float _velocityFallMin;
    [SerializeField] [Tooltip("Gravity when the player goes up and press jump")] float _gravityUpJump;
    [SerializeField] [Tooltip("Gravity otherwise")] float _gravity;
    [SerializeField] float _jumpInputTimer = 0.1f;
    [SerializeField] float _timerNoJump;
    [SerializeField] float _timerSinceJumpPressed;
    [SerializeField] float _TimeSinceGrounded;

    [Header("Anim")]
    public GameObject PlayerMesh;

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

    public int NumberTeleport = 1;
    public Teleport _teleport;
    private PlayerInputs _input;
    public bool teleportationClick;
    public Vector2 _cursor;
    public bool IsGamepad;
    public AudioManager Audio;
    public string _walkSound;
    public string _jumpSound;

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
        _inputs = move.ReadValue<Vector2>();

        Audio.Play(_walkSound);
    }
    
    void GetCursorInputsGamepad(InputAction.CallbackContext cursor)
    {
        IsGamepad = true;
    }
    
    void GetCursorInputsMouse(InputAction.CallbackContext cursor)
    {
        IsGamepad = false;
    }

    void GetJumpInputs(InputAction.CallbackContext jump)
    {
        _inputJump = true;
        _timerSinceJumpPressed = 0;

        Audio.Play(_jumpSound);
    }

    void GetShootInputs(InputAction.CallbackContext tp)
    {
        teleportationClick = true;
    }

    void HandleInputs()
    {
        if (IsGamepad && _input.Player.CursorGamepad.ReadValue<Vector2>() != Vector2.zero)
        {
            _cursor = _input.Player.CursorGamepad.ReadValue<Vector2>();
        }
        else if (!IsGamepad)
        {
            _cursor = Camera.main.ScreenToWorldPoint(_input.Player.CursorMouse.ReadValue<Vector2>());
        }

        teleportationClick = false;
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
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point, _groundRadius);
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

            PlayerMesh.transform.localScale = new Vector3(side, PlayerMesh.transform.localScale.y, PlayerMesh.transform.localScale.z);
        }
    }
}
