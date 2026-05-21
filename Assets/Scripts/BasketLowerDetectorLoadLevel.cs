using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BasketLowerDetectorLoadLevel : MonoBehaviour
{
    public int levelIndex;
    private bool scored = false;
    public bool exitApp = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider) {
        if (scored) {
            return;
        }
        if (transform.parent.GetChild(0).GetComponent<BasketDetectorTop>().isActive && collider.CompareTag("Lopta")){
             scored = true;
             StartCoroutine(ScoreSequence());
        }
    }

    IEnumerator ScoreSequence() {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        if (exitApp) {
            QuitGame();
        } else {
            LevelManager.instance.LoadLevel(levelIndex);
        }
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();

}
}
