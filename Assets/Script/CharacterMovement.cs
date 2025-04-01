using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;

    [Header("PuppetMode")]
    public IdleState idleState;
    public RunState runState;
    public AttackState attackState;
    public DashState dashState;
    //public RewindState rewindState;

    [Header("SoulMode")]
    public Soul_AttackState soulAttackState;
    public Soul_IdleState soulIdleState;
    public Soul_RunState soulRunState;
    public Soul_DashState soulDashState;
    [SerializeField]
    PlayerState state;

    public Animator animator;

    public bool ableToAttack = true;
    [HideInInspector]
    public bool isAttacking;
    public float attackCooldown = 0.5f;
    public bool ableToDash = true;
    [HideInInspector]
    public bool isDashing;
    public float dashCooldown = 2f;
    [HideInInspector]
    public bool attackTriggered;
    public bool dashTriggered;
    public bool switchBodyTriggered;
    bool isRewinding;
    public CharacterBodyType.BodyType currentBodyType;
    public float maxSeperateDistance = 20f;

    public event Action OnRewindStarted;
    public UnityEvent onPlayerDamaged;

    public int puppetLifeCount = 3;
    public float moveSpeed = 5f; // Speed of the player
    public float dashDistance = 5f;
    public float dashDuration = 0.4f;
    public float attackcooldown = 0.05f;
    public float facingRotation = 0f;
    public float inputDeadZone = 0.05f;
    public GameObject hitboxParent;
    public CharacterHitbox characterHitbox;
    public GameObject projectilePrefab; // Projectile prefab to instantiate
    public GameObject puppetPrefab;
    [HideInInspector]
    public GameObject currentProjectile;

    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    [HideInInspector]
    public Vector2 movement; // Store movement input
    public Vector2 lastMovement; //Store latest movement input
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
        dashState.Setup(rb, animator, this);
        //rewindState.Setup(rb, animator, this);
        soulAttackState.Setup(rb, animator, this);
        soulIdleState.Setup(rb, animator, this);
        soulRunState.Setup(rb, animator, this);
        soulDashState.Setup(rb, animator, this);
        state = idleState;
        characterHitbox.SetupHitbox(this);
        SwitchPlayerTag();

    }

    void Update()
    {
        GetInput();

        if (state.isCompleted)
        {
            SelectState();
        }

        state.Do();
    }

    void FixedUpdate()
    {
        if (!(state.Equals(runState) || state.Equals(soulRunState))) return;
        // Move the player using the Rigidbody2D component
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void GetInput()
    {
        // Get input from the W, A, S, D keys
        //movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //print(Gamepad.current[GamepadButton.LeftStick]);

        if (Mathf.Abs(movement.x) < inputDeadZone) movement.x = 0f;
        if (Mathf.Abs(movement.y) < inputDeadZone) movement.y = 0f;

        if (movement != Vector2.zero)
        {
            lastMovement = movement;
            facingRotation = Vector2ToRotation(movement);
            hitboxParent.transform.rotation = Quaternion.Euler(0f, 0f, facingRotation);
        }

        pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDashing || isAttacking) return;
        if (Input.GetMouseButtonDown(1) && ableToDash)
        {
            dashTriggered = true;
        }
        else if (Input.GetMouseButtonDown(0) && ableToAttack)
        {
            attackTriggered = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switchBodyTriggered = true;
        }
    }

    void SelectState()
    {
        //Player cannot move when rewinding or attacking
        /* if (isRewinding)
        {
            state = rewindState;
        }
        
        else */

        if (dashTriggered || isDashing)
        {
            if (currentBodyType == CharacterBodyType.BodyType.Physical) //dash as puppet
            {
                state = dashState;
            }
            else //dash as soul
            {
                state = soulDashState;
            }
        }
        else if (switchBodyTriggered)
        {
            if (currentBodyType == CharacterBodyType.BodyType.Physical) //switch to soul
            {
                state = soulIdleState;
            }
            else //switch to puppet
            {
                state = idleState;
            }
        }
        else if (attackTriggered || isAttacking)
        {
            if (currentBodyType == CharacterBodyType.BodyType.Physical) //attack as puppet
            {
                state = attackState;
            }
            else //attack as soul
            {
                state = soulAttackState;
            }
        }
        else if ((Mathf.Abs(movement.x) + Mathf.Abs(movement.y)) > 0f)
        {
            if (currentBodyType == CharacterBodyType.BodyType.Physical) //move as puppet
            {
                state = runState;
            }
            else //move as soul
            {
                state = soulRunState;
            }
        }
        else
        {
            if (currentBodyType == CharacterBodyType.BodyType.Physical) //idle as puppet
            {
                state = idleState;
            }
            else //idle as soul
            {
                state = soulIdleState;
            }
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
        puppetLifeCount--;
        /*if ((currentBodyType == CharacterBodyType.BodyType.Soul && (type == AttackType.EnemyAttackType.Soul || type == AttackType.EnemyAttackType.Both)) ||
            (currentBodyType == CharacterBodyType.BodyType.Physical && (type == AttackType.EnemyAttackType.Physical || type == AttackType.EnemyAttackType.Both)))
        {
            puppetLifeCount--;
        }*/

        if (puppetLifeCount <= 0)
        {
            //For testing only
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StartRewindActions()
    {
        OnRewindStarted?.Invoke();
    }

    public void ForceReturnPuupet()
    {
        switchBodyTriggered = true;
    }

    public void SwitchPlayerTag()
    {
        print(currentBodyType.ToString());
        gameObject.tag = currentBodyType.ToString() + "Player";
    }

    float Vector2ToRotation(Vector2 direction)
    {
        // Normalize the vector
        direction.Normalize();

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }
}
