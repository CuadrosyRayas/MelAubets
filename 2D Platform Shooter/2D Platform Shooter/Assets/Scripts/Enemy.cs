using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public LayerMask enemyMask;
    public float speed = 1;
    private Rigidbody2D enemyBody;
    Transform enemyTrans;
    float width, height;
    public Animator animator;

    void Start()
    {
        enemyTrans = this.transform;
        enemyBody = this.GetComponent<Rigidbody2D>();
        width = this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTrans.position.toVector2() - enemyTrans.right.toVector2() * width;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - enemyTrans.right.toVector2() * .02f);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - enemyTrans.right.toVector2(), enemyMask);

        if (!isGrounded || isBlocked)
        {
            Vector3 currRotation = enemyTrans.eulerAngles;
            currRotation.y += 180;
            enemyTrans.eulerAngles = currRotation;
        }

        Vector2 enemyVel = enemyBody.velocity;
        enemyVel.x = -enemyTrans.right.x * speed;
        enemyBody.velocity = enemyVel;
    }

    public void Hurt()
    {
        animator.SetBool("IsDead", true);
        speed = 0;
        Destroy(this.gameObject, 0.35f);
    }
}
