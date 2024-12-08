using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D _enemyRigidBody;

    private void Start()
    {
        _enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _enemyRigidBody.linearVelocity = new Vector2(moveSpeed, 0f); 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }
    
    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Math.Sign(_enemyRigidBody.linearVelocity.x), 1f);
    }
}
