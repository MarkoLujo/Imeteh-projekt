using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float trailDuration = 1f;
    private bool active = false;

    private void OnTriggerEnter(Collider col) {
        if (active) return;
        if (col.CompareTag("Lopta")){
            TrailRenderer trail = col.GetComponent<TrailRenderer>();
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (trail != null && rb != null) {
                active = true;
                StartCoroutine(ShowTrail(trail, rb));
            }
        }
    }

    IEnumerator ShowTrail(TrailRenderer trail, Rigidbody rb) {
        trail.Clear();   
        float speed = rb.linearVelocity.magnitude;
        trail.time = Mathf.Clamp(speed / 10f, 0.2f, 1f);

        yield return new WaitForSeconds(1f);

        trail.time = 0f;
        active = false;
    }
}
