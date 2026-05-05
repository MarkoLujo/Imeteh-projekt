using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public class Throwable : InteractableUnityEventWrapper
{
    Queue<Vector3> trackedPositions = new Queue<Vector3>();
    bool isGrabbed = false;
    public int maxStoredPositions = 4;
    public int predictAhead = 2;



    //[Range (0,1)]
    //public int throwType = 0;

    void FixedUpdate() {
        if (isGrabbed) { 
            trackedPositions.Enqueue(GetComponent<Rigidbody>().position);

            if (trackedPositions.Count > maxStoredPositions) { 
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

        if (trackedPositions.Count > 0 ){
            Vector3 pos1;
            Vector3 pos2;

            pos1 = trackedPositions.Dequeue();

            List<Vector3> velocities = new();

            while (trackedPositions.Count > 0)
            {
                pos2 = trackedPositions.Dequeue();
            
                velocities.Add((pos2-pos1) / Time.fixedDeltaTime );
            
                pos1 = pos2;
            }

            pos2 = GetComponent<Rigidbody>().position;
            velocities.Add(pos2-pos1);

            if (velocities.Count > 1){


                // Stari naèin, loše radi

                /* 

                Vector3 acceleration = Vector3.zero;
                Vector3 velocity = Vector3.zero;

                velocity += velocities[0];

                for (int i = 1; i < velocities.Count; i++){
                    velocity += velocities[i];
                    acceleration += ((velocities[i] - velocities[i-1]) / Time.fixedDeltaTime);
                }

                acceleration /= (velocities.Count - 1);
                velocity /= velocities.Count;

                Debug.Log("Vel:" + velocities[velocities.Count-1] + ", Acc:" + acceleration);
                //GetComponent<Rigidbody>().linearVelocity = vel2 + (vel2 - vel1) * (0.0f/counter);

                GetComponent<Rigidbody>().linearVelocity = velocity + acceleration * Time.fixedDeltaTime * max_stored_positions;
                */
                
                
                // Linearna regresija ili nešto takvo

                // Sredina brzine i vremena
                Vector3 avgVelocity = Vector3.zero;
                float avgTime = 0;

                for (int i = 0; i < velocities.Count; i++){
                    avgVelocity += velocities[i];
                    avgTime += i;
                }
                avgVelocity /= velocities.Count;
                avgTime /= velocities.Count;

                // y = b0 + b1 * x
                //B1 = sum((x(i) - mean(x)) * (y(i) - mean(y))) / sum( (x(i) - mean(x))^2 )
                //B0 = mean(y) - B1 * mean(x)

                //  vel = b0 + b1 * t
                //  B1 = sum(
                //        ( t[i] - avg(t) ) * ( vel[i] - avg(vel) )
                //     )

                //     /
                //
                //     sum(
                //        ( t[i] - avg(t) )^2
                //     )

                // B0 = avg(vel) - B1 * avg(t)

                float sum1X = 0;
                float sum1Y = 0;
                float sum1Z = 0;
                float sum2 = 0;

                for (int i = 0; i < velocities.Count; i++){
                    sum1X += (i - avgTime) * (velocities[i].x - avgVelocity.x);
                    sum1Y += (i - avgTime) * (velocities[i].y - avgVelocity.y);
                    sum1Z += (i - avgTime) * (velocities[i].z - avgVelocity.z);
                    sum2 += (i - avgTime) * (i - avgTime);
                }

                Vector3 acceleration = new Vector3(sum1X / sum2, sum1Y / sum2, sum1Z / sum2);
                Vector3 startingVelocity = avgVelocity - acceleration * avgTime;

                Vector3 velocity = startingVelocity + acceleration * Time.fixedDeltaTime * (maxStoredPositions + predictAhead);


                Debug.Log("Vel:" + velocity + ", Acc:" + acceleration);

                GetComponent<Rigidbody>().linearVelocity = velocity;
                

            }
            else{
                    
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        else{
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        

        isGrabbed = false;
    }
}
