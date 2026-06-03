using System.Runtime.CompilerServices;
using UnityEngine;

public class BasketSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip scoreSound;
    public AudioClip swishSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision col)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swishSound);

    }

    // Update is called once per frame
}
