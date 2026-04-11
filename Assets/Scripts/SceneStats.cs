using UnityEngine;

public class SceneStats : MonoBehaviour
{
    public static SceneStats instance;

    public int points;
    public TextMesh scoreDisplay;

    private void Start(){
        instance = this;
        points = 0;
    }

    public void Score() { 
        points++;
        scoreDisplay.text = points.ToString();
    }
}
