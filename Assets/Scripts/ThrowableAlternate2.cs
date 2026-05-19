using Oculus.Interaction;
using UnityEngine;

public class ThrowableAlternate2 : IThrowable
{
    [Header("Throw Settings")]
    public float throwMultiplier = 1.2f;

    bool isGrabbed = false;



    public override void Grab() { 

    }

    public override void Throw(){
       
        // (opcionalno) dodaj “throw feeling”
        Vector3 velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);

        GetComponent<Rigidbody>().linearVelocity = velocity * throwMultiplier;
        GetComponent<Rigidbody>().angularVelocity = angularVelocity;

        Debug.Log("Vel:" + GetComponent<Rigidbody>().linearVelocity);
    }
}
