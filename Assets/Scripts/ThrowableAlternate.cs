using Oculus.Interaction;
using UnityEngine;

public class ThrowableAlternate : IThrowable
{
    bool isGrabbed = false;

    public GameObject shadowPrefab;

    GameObject simulatedBall;

    public float forceMultiplier = 2f;

    [Range (0,1)]
    public float damping = 0.01f;

    public bool showSimulatedBall = true;


    public void ChangeForceMultiplier(float value){
        Debug.Log("Sila promijenjena na: " + value);
        forceMultiplier = value;
    }

    public void ChangeDamping(float value){
        Debug.Log("Prigušenje promijenjeno na: " + value);
        damping = value;
    }

    public void ChangeSimulatedDisplay(bool value){
        showSimulatedBall = value;
    }
    
    void FixedUpdate() {
        if (isGrabbed) { 
            
            simulatedBall.GetComponent<Rigidbody>().AddForce((transform.position - simulatedBall.transform.position)*forceMultiplier);
            simulatedBall.GetComponent<Rigidbody>().linearVelocity *= 1.0f - damping;
        }
        else{

        }
    }

    public override void Grab() { 
        isGrabbed = true;
        simulatedBall = Instantiate(shadowPrefab, transform.position, transform.rotation);
        if (!showSimulatedBall) { 
            simulatedBall.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void Throw(){
       
        GetComponent<Rigidbody>().linearVelocity = simulatedBall.GetComponent<Rigidbody>().linearVelocity;
        Destroy(simulatedBall);

        Debug.Log("Vel:" + GetComponent<Rigidbody>().linearVelocity);
        isGrabbed = false;
    }


    void OnDestroy() {
        Destroy(simulatedBall);
    }
}
