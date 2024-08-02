using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpVelocity = 6f;
    private Rigidbody2D rigidBody;
    private PlayerControls playerControls;
    private float horizontalMovement;

    //Ground Check
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(2f, 0.1f);
    [SerializeField] private Transform groundCheckPos;
    private bool isGrounded;

    //animation fields
    private bool isWalking;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        rigidBody = GetComponent<Rigidbody2D>();

        // Link methods to input actions
        playerControls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>().x);
        playerControls.Player.Jump.performed += ctx => Jump();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        horizontalMovement = moveInput.x;

        // Apply movement
        rigidBody.velocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.velocity.y);

        // Check if player is walking
        isWalking = Mathf.Abs(horizontalMovement) > 0.01f;

        //Rotate player model on direction change
        if (horizontalMovement > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalMovement < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Check for grounded state
        isGrounded = groundCheck();
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


    public bool IsJumping() {
        return isJumping;
    }
}
