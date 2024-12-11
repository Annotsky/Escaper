using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 20f;
    private Rigidbody2D _arrowRigidBody;
    PlayerMovement _player;
    private float _xSpeed;
    
    void Start()
    {
        _arrowRigidBody = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _xSpeed = _player.transform.localScale.x * arrowSpeed;
        transform.localScale = new Vector2(Mathf.Sign(_xSpeed) * transform.localScale.x, transform.localScale.y);
    }

    void Update()
    {
        _arrowRigidBody.linearVelocity = new Vector2(_xSpeed, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
