using UnityEngine;

public class BallCreateAndReset : MonoBehaviour
{
    [Header("References")]
    public GameObject ballPrefab;
    public Transform handTransform;

    [Header("Throw Settings")]
    public float throwMultiplier = 1.2f;

    private GameObject currentBall;
    private Rigidbody currentBallRb;

    private bool holdingBall = false;
    private bool gripWasHeld = false;

    void Update()
    {
        bool yPressed = OVRInput.GetDown(OVRInput.Button.Four); // Y button
        bool gripHeld = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger); // left grip

        // 1. SPAWN / GRAB (Y + grip)
        if (yPressed && gripHeld)
        {
            SpawnAndGrabBall();
        }

        // 2. RELEASE (pusti grip)
        if (holdingBall && gripWasHeld && !gripHeld)
        {
            ReleaseBall();
        }

        gripWasHeld = gripHeld;
    }

    public void DeleteBall() { 
        // makni staru loptu ako postoji
        if (currentBall != null)
        {
            Destroy(currentBall);
        }
    }

    void SpawnAndGrabBall()
    {
        DeleteBall();

        // spawn nove lopte
        currentBall = Instantiate(ballPrefab);
        currentBallRb = currentBall.GetComponent<Rigidbody>();

        // disable physics dok je u ruci
        currentBallRb.isKinematic = true;

        // attach na ruku
        currentBall.transform.SetParent(handTransform);
        currentBall.transform.localPosition = Vector3.zero;
        currentBall.transform.localRotation = Quaternion.identity;

        holdingBall = true;
    }

    void ReleaseBall()
    {
        if (currentBall == null) return;

        // detach
        currentBall.transform.SetParent(null);

        // enable physics
        currentBallRb.isKinematic = false;

        // (opcionalno) dodaj “throw feeling”
        Vector3 velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);

        currentBallRb.linearVelocity = velocity * throwMultiplier;
        currentBallRb.angularVelocity = angularVelocity;

        holdingBall = false;
    }
}