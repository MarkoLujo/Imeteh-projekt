using UnityEngine;

public class BasketMove : MonoBehaviour
{
    public float delta;
    public float speed;
    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin (Time.time * speed);
        transform.position = v;
    }
}
