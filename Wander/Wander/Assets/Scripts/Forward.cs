using UnityEngine;

// Computes the angle between the target transform and this object

public class Forward : MonoBehaviour
{
    public float angleBetween = 0.0f;
    public Transform target;

    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        angleBetween = Vector3.Angle(transform.forward, targetDir);
    }
}
