using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climbSpeed = 1f;

    private Vector2 _moveInput;
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private BoxCollider2D _playerCollider;
    private float _gravityScaleAtStart;


    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _myRigidbody.gravityScale;
    }

    private void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        if (!_playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            _myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _myRigidbody.linearVelocity.y);
        _myRigidbody.linearVelocity = playerVelocity;
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_myRigidbody.linearVelocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_myRigidbody.linearVelocity.x), 1f);
            _myAnimator.SetBool("isRunning", true);
        }
        else _myAnimator.SetBool("isRunning", false);
    }

    private void ClimbLadder()
    {
        
        if (!_playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            _myRigidbody.gravityScale = _gravityScaleAtStart;
            _myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2(_myRigidbody.linearVelocity.x, _moveInput.y * climbSpeed);
        _myRigidbody.linearVelocity = climbVelocity;
        _myRigidbody.gravityScale = 0f;
        
        bool playerHasVerticalSpeed = Mathf.Abs(_myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
