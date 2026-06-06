using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public class BasketSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip scoreSound;
    public ParticleSystem scoreParticles; 
    public AudioClip swishSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Lopta"){
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swishSound);
        if(transform.parent.GetChild(0).GetComponent<BasketDetectorTop>().isActive){
            StartCoroutine(PlayScore());
        }
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
