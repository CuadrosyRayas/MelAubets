using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    public bool isFlipped = false;
    public bool isJumping = false;
    public bool isIdle =true;

    public float jumpForceX = 10f;
    public float jumpForceY = 30f;

    public float lastYPos = 0;

    public Animator animator;

    public Transform player;
    public Rigidbody2D rb;

    public SpriteRenderer sprite;

    public float idleTime = 2f;
    public float currentIdleTime = 0;

    public int Health = 10;

    void Start()
    {
        lastYPos = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        animator.SetFloat("Speed", rb.velocity.y);
        if (isIdle)
        {
            currentIdleTime += Time.deltaTime;
            if(currentIdleTime>= idleTime)
            {
                currentIdleTime = 0;
                Jump();
            }
        }

        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void Jump()
    {
        int direction = (int) (transform.position.x / Mathf.Abs(transform.position.x));
        if(isFlipped == false)
            rb.velocity = new Vector2(jumpForceX * -direction, jumpForceY);
        else
            rb.velocity = new Vector2(jumpForceX * direction, jumpForceY);
    }

    public void Hurt()
    {
        Health--;
        Score.scoreValue += 100;
        StartCoroutine(FlashRed());
        if (Health <= 0)
        {
            Destroy(this.gameObject, 0.35f);
        }
    }

    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
