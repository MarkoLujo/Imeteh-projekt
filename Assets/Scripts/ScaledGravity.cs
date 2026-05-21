using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ScaledGravity : MonoBehaviour
{
    [Tooltip("1 = normal gravity, 0.2 = light/floaty ball, 0 = no gravity.")]
    [Range(0f, 2f)]
    public float gravityScale = 0.25f;

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
            rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }
    }
}
