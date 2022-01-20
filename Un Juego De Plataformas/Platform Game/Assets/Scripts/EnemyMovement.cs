using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Enemy controller;
    public Animator animator;

    public void IsDead()
    {
        animator.SetBool("IsDead", true);
    }
}
