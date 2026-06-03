using UnityEngine;

public class BallBounceSound : MonoBehaviour
{
    public AudioClip[] bounce_clips;
    private AudioSource audioSource;
    public float maxImpactVelocity = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision col){
        float impact = col.relativeVelocity.magnitude;
        float volume = Mathf.Clamp01(impact / maxImpactVelocity);
        //ako su mali udarci
        if (volume < 0.1f) return;
        int index = Random.Range(0, bounce_clips.Length);

        //change pitch
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(bounce_clips[index], volume);
    }
}
