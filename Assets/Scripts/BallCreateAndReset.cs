using UnityEngine;

public class BallCreateAndReset : MonoBehaviour
{
    [Header("References")]
    public GameObject ballPrefab;
    public Transform handTransform;



    private GameObject currentBall;
    private Rigidbody currentBallRb;

    private bool holdingBall = false;
    private bool gripWasHeld = false;

    void Update()
    {
        bool yPressed = OVRInput.GetDown(OVRInput.Button.One); // Y button
        bool gripHeld = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger); // left grip

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

        // disable physics dok je u ruci - simuliraj metin grab sustav
        currentBallRb.isKinematic = true;

        currentBall.GetComponent<IThrowable>().Grab();
        // attach na ruku
        currentBall.transform.SetParent(handTransform);
        currentBall.transform.localPosition = new Vector3(-0.07f,0,0);
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
        currentBall.GetComponent<IThrowable>().Throw();
        holdingBall = false;
    }
}