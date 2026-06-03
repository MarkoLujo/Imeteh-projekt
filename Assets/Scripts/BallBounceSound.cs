using UnityEngine;

public class BallBounceSound : MonoBehaviour
{
    public AudioClip[] bounce_clips;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision col){
        int index = Random.Range(0, bounce_clips.Length);

        //change pitch
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(bounce_clips[index]);
    }
}
