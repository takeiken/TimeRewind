using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;

    public IdleState idleState;
    public RunState runState;
    public AttackState attackState;
    public RewindState rewindState;
    [SerializeField]
    State state;

    public Animator animator;

    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool attackTriggered;
    bool isRewinding;

    public event Action OnRewindStarted;
    public UnityEvent onPlayerDamaged;

    public int lifeCount = 3;
    public float moveSpeed = 5f; // Speed of the player
    public float attackcooldown = 0.05f;
    public GameObject projectilePrefab; // Projectile prefab to instantiate
    [HideInInspector]
    public GameObject currentProjectile;

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    [HideInInspector]
    public Vector2 movement; // Store movement input
    private Vector2 pointerPos;

    public SpriteRenderer sprite;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
           Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        idleState.Setup(rb, animator, this);
        runState.Setup(rb, animator, this);
        attackState.Setup(rb, animator, this);
        rewindState.Setup(rb, animator, this);
        state = idleState;
    }

    void Update()
    {
        GetInput();

        if (state.isCompleted)
        {
            SelectState();
        }

        state.Do();
        attackTriggered = false;
        //UpdateState();
    }

    void FixedUpdate()
    {
        if (!state.Equals(runState)) return;
        // Move the player using the Rigidbody2D component
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void GetInput()
    {
        // Get input from the W, A, S, D keys
        //movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            attackTriggered = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAttacking)
        {
            isRewinding = true;
        }
        
        if (currentProjectile.IsDestroyed())
        {
            isRewinding = false;
        }
    }

    void SelectState()
    {
        //Player cannot move when rewinding or attacking
        if (isRewinding)
        {
            state = rewindState;
        }
        else if (attackTriggered || isAttacking)   
        {
            state = attackState;
        }
        else if ((Mathf.Abs(movement.x) + Mathf.Abs(movement.y)) > 0f)
        {
            state = runState;
        }
        else
        {
            state = idleState;
        }

        state.Initialize();
        state.Enter();
    }

    public Vector2 GetCursorPosition()
    {
        return pointerPos;
    }

    public void PlayerDamagedActions()
    {
        lifeCount--;
    }

    public void StartRewindActions()
    {
        OnRewindStarted?.Invoke();
    }
}
