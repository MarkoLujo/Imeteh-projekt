using UnityEngine;

public class SpinHoop : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, 0.5f);
    }
}
