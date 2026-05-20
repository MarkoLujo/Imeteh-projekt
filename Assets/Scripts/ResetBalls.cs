using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ResetBalls : MonoBehaviour
{
    public List<Transform> resetObjects = new List<Transform>();
    private List<Vector3> start_positions = new List<Vector3>();
    private List<Quaternion> start_rotations = new List<Quaternion>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start(){
        foreach (Transform obj in resetObjects)
        {
            start_positions.Add(obj.position);
            start_rotations.Add(obj.rotation);
        }
    }

    public void ResetAllObj(){
        for(int i = 0;i< resetObjects.Count; i++){
            resetObjects[i].position = start_positions[i];
            resetObjects[i].rotation = start_rotations[i];
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
    //public void ResetScene(){
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
}
