using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climbSpeed = 1f;
    [SerializeField] private Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform gun;

    private Vector2 _moveInput;
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private CapsuleCollider2D _playerBodyCapsuleCollider2D;
    private BoxCollider2D _playerFeet;
    private float _gravityScaleAtStart;
    
    private bool _isAlive = true;


    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _playerBodyCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _playerFeet = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _myRigidbody.gravityScale;
    }

    private void Update()
    {
        if (!_isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnFire(InputValue value)
    {
        if (!_isAlive) { return; }

        Instantiate(arrow, gun.position, transform.rotation);
    }

    private void OnMove(InputValue value)
    {
        if (!_isAlive) { return; }
        _moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        if (!_isAlive) { return; }
        if (!_playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

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
        
        if (!_playerFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
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
    
    private void Die()
    {
        if (_playerBodyCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Spikes")))
        {
            _isAlive = false;
            _myAnimator.SetTrigger("Dying");
            _myRigidbody.linearVelocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
