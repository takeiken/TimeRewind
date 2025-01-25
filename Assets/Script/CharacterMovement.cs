using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;

    public event Action OnRewindStarted;
    public UnityEvent onPlayerDamaged;

    public int lifeCount = 3;
    public float moveSpeed = 5f; // Speed of the player
    public GameObject projectilePrefab; // Projectile prefab to instantiate
    private GameObject currentProjectile;

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store movement input
    private Vector2 pointerPos;

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
            //GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Get input from the W, A, S, D keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D keys or Left/Right arrow keys
        movement.y = Input.GetAxisRaw("Vertical"); // W/S keys or Up/Down arrow keys
        pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && currentProjectile == null)
        {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.Space) && !currentProjectile.IsDestroyed())
        {
            OnRewindStarted?.Invoke();
        }
    }

    void FixedUpdate()
    {
        // Move the player using the Rigidbody2D component
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        // Create a projectile instance
        currentProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction to the cursor
        Vector2 direction = (GetCursorPosition() - rb.position).normalized;

        // Set the projectile's rotation to face the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        currentProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        currentProjectile.GetComponent<SimpleProjectile>().SetCharacter(this);
    }

    public Vector2 GetCursorPosition()
    {
        return pointerPos;
    }

    public void PlayerDamagedActions()
    {
        lifeCount--;
    }
}
