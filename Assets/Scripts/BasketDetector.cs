using UnityEngine;

public class BasketDetector : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider collider) {

        if (transform.parent.GetChild(0).GetComponent<BasketDetectorTop>().isActive && collider.CompareTag("Lopta")){
            SceneStats.instance.Score();
            Debug.Log("Score");
        }
    }
}
