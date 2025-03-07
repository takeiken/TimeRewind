using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.Rendering.DebugUI;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;

    [Header("PuppetMode")]
    public IdleState idleState;
    public RunState runState;
    public AttackState attackState;
    public RewindState rewindState;

    [Header("SoulMode")]
    public Soul_AttackState soulAttackState;
    public Soul_IdleState soulIdleState;
    public Soul_RunState soulRunState;
    [SerializeField]
    State state;

    public Animator animator;

    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool attackTriggered;
    public bool switchBodyTriggered;
    bool isRewinding;
    public CharacterBodyType.BodyType currentBodyType;

    public event Action OnRewindStarted;
    public UnityEvent onPlayerDamaged;

    public int puppetLifeCount = 3;
    public float moveSpeed = 5f; // Speed of the player
    public float attackcooldown = 0.05f;
    public float facingRotation = 0f;
    public float inputDeadZone = 0.05f;
    public GameObject hitboxParent;
    public CharacterHitbox characterHitbox;
    public GameObject projectilePrefab; // Projectile prefab to instantiate
    public GameObject puppetPrefab;
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
        soulAttackState.Setup(rb, animator, this);
        soulIdleState.Setup(rb, animator, this);
        soulRunState.Setup(rb, animator, this);
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
        attackTriggered = false;
        //UpdateState();
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
            facingRotation = Vector2ToRotation(movement);
            hitboxParent.transform.rotation = Quaternion.Euler(0f, 0f, facingRotation);
        }

        pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            attackTriggered = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            switchBodyTriggered = true;
        }

        /*if (Input.GetKeyDown(KeyCode.Space) && isAttacking)
        {
            isRewinding = true;
        }*/

        /*if (currentProjectile.IsDestroyed())
        {
            isRewinding = false;
        }*/
    }

    void SelectState()
    {
        //Player cannot move when rewinding or attacking
        /* if (isRewinding)
        {
            state = rewindState;
        }
        
        else */

        if (switchBodyTriggered)
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
