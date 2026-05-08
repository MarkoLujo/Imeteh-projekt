using UnityEngine;
using TMPro;

public class SceneStats : MonoBehaviour
{
    public static SceneStats instance;

    public int points;
    public TextMeshProUGUI scoreDisplay;


    private void Start(){
        instance = this;
        points = 0;
    }

    public void Score() { 
        points++;
        int displayPoints = Mathf.Min(points, 99);
        if (displayPoints < 10){
            scoreDisplay.text = " " + displayPoints.ToString();
        } else {
            scoreDisplay.text = points.ToString();
        }
    }
}
