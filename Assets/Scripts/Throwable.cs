using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : InteractableUnityEventWrapper
{
    Queue<Vector3> trackedPositions = new Queue<Vector3>();
    bool isGrabbed = false;
    const int max_stored_positions = 10;

    void Update() {
        if (isGrabbed) { 
            trackedPositions.Enqueue(GetComponent<Rigidbody>().linearVelocity);

            if (trackedPositions.Count > max_stored_positions) { 
                trackedPositions.Dequeue();
            }
        }
        else{
            trackedPositions.Clear();
        }
    }

    public void Grab() { 
        isGrabbed = true;
    }

    public void Throw(){
        Debug.Log("Thrown");
        Vector3 vel1;
        Vector3 vel2;

        int counter = 0;
        vel1 = trackedPositions.Dequeue();
        vel2 = vel1;
        while (trackedPositions.Count > 0 && counter < max_stored_positions) { 
            vel2 = trackedPositions.Dequeue();
            counter++;
        }
        

        GetComponent<Rigidbody>().linearVelocity = vel2 + (vel2-vel1);

        isGrabbed = false;
    }
}
