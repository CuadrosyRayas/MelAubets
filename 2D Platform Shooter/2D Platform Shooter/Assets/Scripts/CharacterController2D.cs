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
    public Transform Crank;
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

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
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
        Debug.Log(Health);
        Enemy enemy = coll.collider.GetComponent<Enemy>();
        Boss boss = coll.collider.GetComponent<Boss>();
        if (enemy != null)
        {
            foreach (ContactPoint2D point in coll.contacts)
            {
                StartCoroutine(Dead());
            }
        }

        if (boss != null)
        {
            foreach (ContactPoint2D point in coll.contacts)
            {
                StartCoroutine(Hurt());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        CherryMovement cherry = trig.GetComponent<CherryMovement>();
        GemMovement gem = trig.GetComponent<GemMovement>();
        Pole pole = trig.GetComponent<Pole>();
        Ramp1 ramp1 = trig.GetComponent<Ramp1>();
        Ramp2 ramp2 = trig.GetComponent<Ramp2>();
        Crank crank = trig.GetComponent<Crank>();
        Vacuum vacuum = trig.GetComponent<Vacuum>();
        TreeFunction tree = trig.GetComponent<TreeFunction>();

        if(crank != null)
        {
            crank.Touched();
        }
        if (ramp1 != null)
        {
            Rigidbody2D.freezeRotation = false;
        }
        if (ramp2 != null)
        {
            Rigidbody2D.freezeRotation = true;
            transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
        if (cherry != null)
        {
            cherry.Caught();
        }
        if(gem != null)
        {
            gem.Caught();
        }
        if (pole != null)
        {
            EndLevel(true);
        }
        if (vacuum != null)
        {
            StartCoroutine(Dead());
        }
        if(tree != null)
        {
            EndGame(true);
        }
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
            if (Player.position.x >= Crank.position.x)
                Player.position = Crank.position;
            else
                Player.position = StartPosition.position;
        }
        if (Health == 1)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart2);
            if (Player.position.x >= Crank.position.x)
                Player.position = Crank.position;
            else
                Player.position = StartPosition.position;
        }
        if (Health == 0)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart1);
            if (Player.position.x >= Crank.position.x)
                Player.position = Crank.position;
            else
                Player.position = StartPosition.position;
        }
        if (Health < 0)
        {
            EndLevel(false);
        }
    }

    IEnumerator Hurt()
    {
        IsDeadEvent.Invoke();

        yield return new WaitForSeconds(0.3f);

        Health--;

        if (Health == 2)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart3);
        }
        if (Health == 1)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart2);
        }
        if (Health == 0)
        {
            IsAliveEvent.Invoke();
            Destroy(Heart1);
        }
        if (Health < 0)
        {
            EndGame(false);
        }
    }

    void EndLevel(bool win)
    {
        if (win == true)
        {
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
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
}
