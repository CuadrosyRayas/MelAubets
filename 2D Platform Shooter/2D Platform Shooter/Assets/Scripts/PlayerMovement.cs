using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    private Rigidbody2D rb;
    public Animator animator;
    public float runSpeed = 40f;
    public float verticalSpeed = 10f;
    float horizontalMove = 0f;
    bool jump = false;
    public float distance;
    public LayerMask whatIsLadder;
    bool climb = false;
    float verticalMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                climb = true;
                animator.SetBool("IsClimbing", true);
                animator.SetBool("IsJumping", false);
            }
        }
        else
        {
            climb = false;
            animator.SetBool("IsClimbing", false);
        }

        if(climb == true)
        {
            verticalMove = Input.GetAxisRaw("Vertical") * verticalSpeed;
            animator.SetFloat("ClimbSpeed", (Mathf.Abs(verticalMove) + Mathf.Abs(horizontalMove)));
            rb.velocity = new Vector2(rb.velocity.x, verticalMove);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
            climb = false;
            animator.SetBool("IsClimbing", false);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsClimbing", false);
    }

    public void IsDead()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsClimbing", false);
        animator.SetBool("IsDead", true);
        runSpeed = 0f;
    }

    public void IsAlive()
    {
        animator.SetBool("IsDead", false);
        runSpeed = 40f;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
