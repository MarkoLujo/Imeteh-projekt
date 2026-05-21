using UnityEngine;

public class OrbitingObstacle : MonoBehaviour
{
    [Header("Orbit")]
    public Transform center;
    public Vector3 axis = Vector3.up;
    public float speedDegreesPerSecond = 70f;

    [Header("Self Rotation")]
    public Vector3 selfRotation = new Vector3(0f, 120f, 0f);

    void Update()
    {
        if (center != null)
        {
            transform.RotateAround(
                center.position,
                axis.normalized,
                speedDegreesPerSecond * Time.deltaTime
            );
        }

        transform.Rotate(selfRotation * Time.deltaTime, Space.Self);
    }
}
