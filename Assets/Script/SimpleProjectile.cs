using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    private CharacterMovement character;

    public float speed = 10f; // Speed of the projectile
    private float currentLifeTime = 0f;
    public float maxLifeTime = 5f;
    private Rigidbody2D rb;

    //rewind list
    private List<Vector2> rewindPos = new List<Vector2>();

    public void SetCharacter(CharacterMovement cm)
    {
        character = cm;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        character.OnRewindStarted += StartRewind;
        rb.linearVelocity = transform.up * speed; // Move the projectile in the direction it's facing
    }

    void FixedUpdate()
    {
        if (speed <= 0f)
        {
            if (rewindPos.Count > 0)
            {
                rb.MovePosition(rewindPos[^1]);
                rewindPos.Remove(rewindPos[^1]);
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        // Ensure the projectile continues moving in its current direction
        rb.MovePosition(rb.position + rb.linearVelocity * Time.fixedDeltaTime);
        rewindPos.Add(rb.position);
        currentLifeTime += Time.fixedDeltaTime;
        if( currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void StartRewind()
    {
        if (speed == Math.Abs(speed))
        {
            speed *= -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect the projectile's direction based on the normal of the collision surface
        Vector2 normal = collision.contacts[0].normal;
        Vector2 newDirection = Vector2.Reflect(rb.linearVelocity, normal).normalized;

        // Update the velocity to bounce off
        rb.linearVelocity = newDirection * speed;
    }
}
