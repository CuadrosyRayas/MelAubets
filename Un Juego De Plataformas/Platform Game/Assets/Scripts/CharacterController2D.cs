using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float JumpForce = 500f;
    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .0005f;
    [SerializeField] private bool AirControl = true;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform CeilingCheck;

    public Transform Player;
    public Transform StartPosition;
    const float GroundedRadius = .2f;
    private bool Grounded;
    const float CeilingRadius = .2f;
    private Rigidbody2D Rigidbody2D;
    private bool FacingRight = true;
    private Vector3 velocity = Vector3.zero;
    public ParticleSystem dust;
    public Transform FirePoint;
    public GameObject Bullet;
    public BulletController BulC;
    int Health = 3;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent IsDeadEvent;
    public UnityEvent IsAliveEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
            CreateDust();
        }

        if (IsDeadEvent == null)
            IsDeadEvent = new UnityEvent();
        if (IsAliveEvent == null)
            IsAliveEvent = new UnityEvent();
    }

    private void Update()
    {
        bool wasGrounded = Grounded;
        Grounded = false;

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            if (!FacingRight && BulC.Speed > 0)
            {
                BulC.Speed = -BulC.Speed;
            }
            else if (FacingRight && BulC.Speed < 0)
            {
                BulC.Speed = -BulC.Speed;
            }
            Instantiate(Bullet, FirePoint.position, Quaternion.identity);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }

        }
    }

    public void Move(float move, bool jump)
    {
        int i = 0;

        if (Grounded || AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing);

            if (move > 0 && !FacingRight)
                Flip();
            else if (move < 0 && FacingRight)
                Flip();
        }
        if (Grounded && jump)
        {
            Grounded = false;
            Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
            CreateDust();
            i = 1;
        }
        if (!Grounded && jump && i <= 1)
        {
            Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
            i += 1;
        }
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        CreateDust();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Enemy enemy = coll.collider.GetComponent<Enemy>();
        Pole pole = coll.collider.GetComponent<Pole>();
        if (enemy != null)
        {
            foreach(ContactPoint2D point in coll.contacts)
            {
                Debug.Log(point.normal);
                Debug.DrawLine(point.point, point.point + point.normal, Color.red, 10);
                if (point.normal.y >= 0.7f)
                {
                    Vector2 velocity = Rigidbody2D.velocity;
                    velocity.y = 10;
                    Rigidbody2D.velocity = velocity;
                    StartCoroutine(enemy.Hurt());
                    StartCoroutine(Invencible());

                }
                else
                {
                    StartCoroutine(Dead());
                }
            }
        }
        if (pole != null)
        {
            EndGame(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
            StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        IsDeadEvent.Invoke();

        yield return new WaitForSeconds(1f);

        Health--;

        if(Health == 2)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart3);
            Player.position = StartPosition.position;
        }
        if (Health == 1)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart2);
            Player.position = StartPosition.position;
        }
        if (Health == 0)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart1);
            Player.position = StartPosition.position;
        }
        if (Health < 0)
        {
            EndGame(false);
        }
    }

    void EndGame(bool win)
    {
        if (win == true)
        {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    public IEnumerator Invencible()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");

        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);

        yield return new WaitForSeconds(0.50f);

        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
    }
}
