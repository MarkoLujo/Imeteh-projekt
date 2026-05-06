using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject lopta1;
    public GameObject lopta2;
    public GameObject lopta3;


    public void ResetBallsPos()
    {
        lopta1.transform.position = new Vector3(0,0,0);
        lopta2.transform.position = new Vector3(0.5f,0,0);
        lopta3.transform.position = new Vector3(0,0,0.5f);

        lopta1.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
        lopta2.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
        lopta3.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
    }

}
