using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpVelocity = 6f;
    [SerializeField] private float sprintSpeed = 8f;
    private Rigidbody2D rigidBody;
    private PlayerControls playerControls;
    private float horizontalMovement;

    //Ground Check
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(2f, 0.1f);
    [SerializeField] private Transform groundCheckPos;
    private bool isGrounded;

    // Stamina fields
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaDrainRate = 20f;

    public float MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;

    //animation fields
    private bool isWalking;
    private bool isJumping;
    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;

        // Link methods to input actions
        playerControls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>().x);
        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Sprint.performed += ctx => Sprint(true);
        playerControls.Player.Sprint.canceled += ctx => Sprint(false);
    }

    // Update is called once per frame
    void Update() {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        horizontalMovement = moveInput.x;

        // Calculate movement speed
        float appliedSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Apply movement
        rigidBody.velocity = new Vector2(horizontalMovement * appliedSpeed, rigidBody.velocity.y);

        // Check if the player is walking or sprinting
        isWalking = Mathf.Abs(horizontalMovement) > 0.01f && !isSprinting;
        isSprinting = Mathf.Abs(horizontalMovement) > 0.01f && isSprinting;

        //Rotate player model on direction change
        if (horizontalMovement > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalMovement < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Check for grounded state
        isGrounded = groundCheck();

        // Manage stamina and sprinting
        if (isSprinting) {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0) {
                currentStamina = 0;
                Sprint(false); // Stop sprinting if out of stamina
            }
        } else {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }


    private void Move(float moveValue) {
        // Reset horizontal movement if there's no input
        horizontalMovement = moveValue;
    }

    private void Jump() {
        if (isGrounded) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpVelocity);
            isJumping = true;
        }
    }

    private bool groundCheck() {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)) {
            isJumping = false;
            return true;
        } else {
            isJumping = true;
            return false;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    private void Sprint(bool sprinting) {
        if (sprinting && currentStamina > 0) {
            isSprinting = true;
        } else {
            isSprinting = false;
        }
    }


    public bool IsJumping() {
        return isJumping;
    }
    public bool IsGrounded() {
        return isGrounded;
    }
    public bool IsWalking() {
        return isWalking;
    }
    public bool IsSprinting() {
        return isSprinting;
    }
}
