using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovement : MonoBehaviour
{
    public Animator animator;

    public void Caught()
    {
        animator.SetBool("IsCaught", true);
        Destroy(this.gameObject);
        Score.scoreValue += 25;
    }
}
