using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    public Animator animator;

    public void Touched()
    {
        animator.SetBool("IsTouched", true);
    }
}
