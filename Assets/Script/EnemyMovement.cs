using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public EnemySO enemySO;

    [Header("State")]
    public Enemy_IdleState idleState;
    public Enemy_FollowState followState;
    public Enemy_AttackState attackState;
    public Enemy_DeadState deadState;
    [SerializeField]
    EnemyState state;

    [Header("Parameters")]
    public int healthPoint = 3;
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
    public NavMeshAgent agent;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool seenPlayer;
    public bool canSeePlayer;
    public bool playerInRange;
    public bool isAttacking;

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
            //healthPoint = enemySO.Enemy.Health; //add this in the future
            radius = enemySO.Enemy.ReactLength;
            angle = enemySO.Enemy.ReactAngle;
            speed = enemySO.Enemy.MovementSpeed;
            attackRange = enemySO.Enemy.AttackRange;
            attackFrequency = enemySO.Enemy.AttackFrequency;
            armor = enemySO.Enemy.Armor;
            bodyType = enemySO.Enemy.BodyType;
        }
        if (idleState != null) idleState.Setup(rb, this);
        if (followState != null) followState.Setup(rb, this);
        if (attackState != null) attackState.Setup(rb, this);
        state = idleState;
        playerRef = CharacterMovement.Instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(FOVRoutine());
        InitializeHitboxes();
    }

    private void Update()
    {
        if (state == null) return;

        if (state.isCompleted)
        {
            SelectState();
        }

        state.Do();
    }

    private void FixedUpdate()
    {
        if (state == null) return;
        if (!state.Equals(followState)) //only move in FollowState
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(playerRef.transform.position);
        }

        //Vector2 dir = (playerRef.transform.position - gameObject.transform.position).normalized;
        //rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    void SelectState()
    {
        if (healthPoint <= 0)
        {
            state = deadState;
        }
        else if ((playerInRange && canSeePlayer) || isAttacking)
        {
            state = attackState;
        }
        else if (seenPlayer)
        {
            state = followState;
        }
        else
        {
            state = idleState;
        }

        state.Initialize();
        state.Enter();
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

    public virtual void TriggerAttack()
    {
        //TriggerAttack in Child class
    }

    private void FacePlayer()
    {
        canSeePlayer = true;
        seenPlayer = true;
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