using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_SPRINTING = "IsSprinting";

    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update() {
        animator.SetBool(IS_WALKING, playerMovement.IsWalking());
        animator.SetBool(IS_GROUNDED, playerMovement.IsGrounded());
        animator.SetBool(IS_JUMPING, playerMovement.IsJumping());
        animator.SetBool(IS_SPRINTING, playerMovement.IsSprinting());
    }
}
