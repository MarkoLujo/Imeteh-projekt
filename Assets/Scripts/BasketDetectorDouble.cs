using UnityEngine;

public class BasketDetectorDouble : MonoBehaviour
{
    public BasketDetectorTop topDetector;
    public BasketDetectorTopDelay[] topDetectors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    private void OnTriggerEnter(Collider collider) {

        bool oneDetectorActive = false;

        for (int i = 0; i < topDetectors.Length; i++) {
            if (topDetectors[i].isActive) { 
                oneDetectorActive = true;
                break;
            }
        }

        if (topDetector.isActive && oneDetectorActive && collider.CompareTag("Lopta")){
            SceneStats.instance.Score();
            Debug.Log("Score");
            if (GetComponent<BasketSounds>() != null){
                GetComponent<BasketSounds>().PlayScoreSequence();
            }
        }
    }
}
