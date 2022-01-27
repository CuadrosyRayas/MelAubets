using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFunction : MonoBehaviour
{
    public Collider2D tree;

    void Start()
    {
        tree.enabled = false;
    }

    void FixedUpdate()
    {
        if (GameObject.Find("boss") == null)
        {
            tree.enabled = true;
        }
    }
}
