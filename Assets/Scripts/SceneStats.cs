using UnityEngine;
using TMPro;

public class SceneStats : MonoBehaviour
{
    public static SceneStats instance;

    public int points;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI timerDisplay;
    private float timer;


    private void Start(){
        instance = this;
        points = 0;
        timerDisplay.text = "00 : 00";
    }

    private void Update(){
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerDisplay.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer() { 
        timer = 0;
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
