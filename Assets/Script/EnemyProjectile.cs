using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    private float currentLifeTime = 0f;
    public float maxLifeTime = 5f;
    private Rigidbody2D rb;
    private AttackType.EnemyAttackType attackType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        rb.linearVelocity = transform.up * speed; // Move the projectile in the direction it's facing
        switch (LayerMask.LayerToName(gameObject.layer))
        {
            case "EnemyPhysicalAttack":
                attackType = AttackType.EnemyAttackType.Physical;
                break;

            case "EnemySoulAttack":
                attackType = AttackType.EnemyAttackType.Soul;
                break;

            case "EnemyMixedAttack":
                attackType = AttackType.EnemyAttackType.Both;
                break;
        }
    }

    void FixedUpdate()
    {
        // Ensure the projectile continues moving in its current direction
        rb.MovePosition(rb.position + rb.linearVelocity * Time.fixedDeltaTime);
        currentLifeTime += Time.fixedDeltaTime;
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("SoulPlayer") && (attackType == AttackType.EnemyAttackType.Soul || attackType == AttackType.EnemyAttackType.Both)) ||
            (collision.CompareTag("PhysicalPlayer") && (attackType == AttackType.EnemyAttackType.Physical || attackType == AttackType.EnemyAttackType.Both)))
        {
            print("Enemy Projectile attack type: " + attackType);
            CharacterMovement.Instance.onPlayerDamaged?.Invoke();
        }

        Destroy(gameObject);
    }
}
