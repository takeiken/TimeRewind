using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemySO enemySO;
    public float radius;
    [Range(0, 360)]
    public float angle;
    public float speed;
    public float attackRange;
    public float attackFrequency;
    public float armor;

    public bool canAttack = true;

    public EnemyHitBox[] hitboxes;
    public CharacterBodyType.BodyType bodyType;

    [HideInInspector]
    public GameObject playerRef;
    public GameObject enemyProjectile;
    [HideInInspector]
    public Rigidbody2D rb;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void OnValidate()
    {
        if (enemySO != null)
        {
            radius = enemySO.Enemy.ReactLength;
            angle = enemySO.Enemy.ReactAngle;
            speed = enemySO.Enemy.MovementSpeed;
            attackRange = enemySO.Enemy.AttackRange;
            attackFrequency = enemySO.Enemy.AttackFrequency;
            armor = enemySO.Enemy.Armor;
            bodyType = enemySO.Enemy.BodyType;
        }
    }

    public virtual void Start()
    {
        if(enemySO!= null)
        {
            radius = enemySO.Enemy.ReactLength;
            angle = enemySO.Enemy.ReactAngle;
            speed = enemySO.Enemy.MovementSpeed;
            attackRange = enemySO.Enemy.AttackRange;
            attackFrequency = enemySO.Enemy.AttackFrequency;
            armor = enemySO.Enemy.Armor;
            bodyType = enemySO.Enemy.BodyType;
        }
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FOVRoutine());
        InitializeHitboxes();
    }

    

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    FacePlayer();
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void FacePlayer()
    {
        canSeePlayer = true;
        // Calculate direction to the player
        Vector2 direction = ((Vector2)CharacterMovement.Instance.transform.position - rb.position).normalized;

        // Set the projectile's rotation to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void InitializeHitboxes()
    {
        hitboxes = transform.GetComponentsInChildren<EnemyHitBox>();
        foreach (EnemyHitBox hitbox in hitboxes)
        {
            hitbox.Setup(this);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Trigger Entered: " + gameObject.name + " ||||| Trigger Tag: " + tag);
        if (collision.CompareTag("CharacterProjectile"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}