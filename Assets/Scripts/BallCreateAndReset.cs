using UnityEngine;

public class BallCreateAndReset : MonoBehaviour
{
    [Header("References")]
    public GameObject ballPrefab;
    public Transform handTransform;



    public GameObject currentBall;
    private Rigidbody currentBallRb;

    private bool holdingBall = false;
    private bool gripWasHeld = false;

    bool yPressed = false;
    bool gripHeld = false;

    void Update()
    {
        yPressed = OVRInput.GetDown(OVRInput.Button.One); // Y button
        gripHeld = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger); // left grip

        // 1. SPAWN / GRAB (Y + grip)
        if (yPressed)
        { 
            SpawnBall();
        }
        /*
        if (yPressed && gripHeld)
        {
            SpawnAndGrabBall();
        }
        */

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

    void SpawnBall() { 
        DeleteBall();
        // spawn nove lopte
        currentBall = Instantiate(ballPrefab);
        currentBallRb = currentBall.GetComponent<Rigidbody>();
        currentBall.transform.SetParent(handTransform);
        currentBall.transform.localPosition = new Vector3(-0.07f,0,0.03f);
        currentBall.transform.localRotation = Quaternion.identity;
        currentBall.transform.SetParent(null);
        currentBall.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,1.5f,0);

        if (gripHeld) { 
            GrabBall();
        }

    }

    void GrabBall() { 
        currentBall.transform.SetParent(handTransform);
        holdingBall = true;
        // disable physics dok je u ruci - simuliraj metin grab sustav
        currentBallRb.isKinematic = true;
        currentBall.GetComponent<IThrowable>().Grab();
        holdingBall = true;
    }
    /*
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
    */

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