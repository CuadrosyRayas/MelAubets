using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    public float Speed;
    public LayerMask bulletMask;
    public Transform bulletTrans;
    float width;
    public CharacterController2D player;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        width = this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody2D.velocity = new Vector2(+Speed, 0);
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Enemy enemy = coll.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            StartCoroutine(enemy.Hurt());
            StartCoroutine(player.Invencible());
        }
    }
}

