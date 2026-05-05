using Oculus.Interaction;
using UnityEngine;

public class ThrowableAlternate : InteractableUnityEventWrapper
{
    bool isGrabbed = false;

    public GameObject shadowPrefab;

    GameObject simulatedBall;

    public float forceMultiplier = 2f;

    [Range (0,1)]
    public float damping = 0.01f;

    //[Range (0,1)]
    //public int throwType = 0;

    void FixedUpdate() {
        if (isGrabbed) { 
            
            simulatedBall.GetComponent<Rigidbody>().AddForce((transform.position - simulatedBall.transform.position)*forceMultiplier);
            simulatedBall.GetComponent<Rigidbody>().linearVelocity *= 1.0f - damping;
        }
        else{

        }
    }

    public void Grab() { 
        isGrabbed = true;
        simulatedBall = Instantiate(shadowPrefab, transform.position, transform.rotation);
    }

    public void Throw(){
       
        GetComponent<Rigidbody>().linearVelocity = simulatedBall.GetComponent<Rigidbody>().linearVelocity;
        Destroy(simulatedBall);

        Debug.Log("Vel:" + GetComponent<Rigidbody>().linearVelocity);
        isGrabbed = false;
    }
}
