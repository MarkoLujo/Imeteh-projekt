using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public class BasketSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip scoreSound;
    public ParticleSystem scoreParticles; 
    public AudioClip swishSound;
    public BasketDetectorTop topDetector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayScoreSequence() { 

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swishSound);
        if(topDetector.isActive){
            StartCoroutine(PlayScore());
        }

    }
    IEnumerator PlayScore(){
        yield return new WaitForSeconds(0.10f);
        audioSource.PlayOneShot(scoreSound);
        if (scoreParticles != null){
            scoreParticles.Play();
        }

    // Update is called once per frame}
    }
}
