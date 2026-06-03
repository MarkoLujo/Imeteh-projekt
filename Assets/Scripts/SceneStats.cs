using UnityEngine;
using TMPro;

public class SceneStats : MonoBehaviour
{
    public static SceneStats instance;

    public int points;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI timerDisplay;
    public float timer;


    private void Start(){
        instance = this;
        points = 0;
    }

    private void Update(){
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        if (!LevelManager.instance.isFreeplay) { 
            timerDisplay.text = $"{minutes:00}:{seconds:00}"; 
        }
    }

    public void ResetTimer() { 
        timer = 0;
    }
    public void ResetScore() { 
        points = 0;
    }

    public void Score() { 
        points++;
        
        int displayPoints = Mathf.Min(points, 99);

        if (scoreDisplay != null) { 
            if (displayPoints < 10){
                scoreDisplay.text = " " + displayPoints.ToString();
            } else {
                scoreDisplay.text = points.ToString();
            }
        }
        LevelManager.instance.OnScore();
    }
}
