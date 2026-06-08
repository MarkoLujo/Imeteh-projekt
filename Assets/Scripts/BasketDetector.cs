using UnityEngine;

public class BasketDetector : MonoBehaviour
{
    public BasketDetectorTop topDetector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    private void OnTriggerEnter(Collider collider) {

        if (topDetector.isActive && collider.CompareTag("Lopta")){
            SceneStats.instance.Score();
            Debug.Log("Score");
            if (GetComponent<BasketSounds>() != null) {
                GetComponent<BasketSounds>().PlayScoreSequence();
            }
        }
    }
}
