using System.Runtime.CompilerServices;
using System.Collections;
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
    void OnTriggerEnter(Collider col)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swishSound);
        StartCoroutine(PlayScore());

    }
    IEnumerator PlayScore(){
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(scoreSound);   
    }

    // Update is called once per frame
}
