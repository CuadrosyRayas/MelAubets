using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMovement : MonoBehaviour
{
    public Animator animator;

    public void Caught()
    {
        animator.SetBool("IsCaught", true);
        Destroy(this.gameObject);
        Score.scoreValue += 5;
    }
}
