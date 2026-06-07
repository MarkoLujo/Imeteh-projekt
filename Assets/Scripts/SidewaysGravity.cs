using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SidewaysGravity : MonoBehaviour
{
    public Vector3 gravity;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
