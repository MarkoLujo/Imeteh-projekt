using UnityEngine;

public class BasketDetectorTop : MonoBehaviour
{

    public bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
    }


    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Lopta")) { 
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Lopta")) { 
            isActive = false;
        }
    }

}
