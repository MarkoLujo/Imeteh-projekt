using System.Collections;
using UnityEngine;

public class BasketDetectorTopDelay : MonoBehaviour
{


    public bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
    }

    IEnumerator Deactivate() {
        yield return new WaitForSeconds(5f);
        isActive = false;
    }


    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Lopta")) { 
            isActive = true;
            Debug.Log("Active!");
            StartCoroutine(Deactivate());
        }
    }

}
