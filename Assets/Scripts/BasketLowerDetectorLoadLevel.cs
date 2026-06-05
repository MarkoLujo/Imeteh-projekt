using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BasketLowerDetectorLoadLevel : MonoBehaviour
{
    public int levelIndex;
    private bool scored = false;
    public bool exitApp = false;
    public bool freeplay = false;
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
        yield return StartCoroutine(SmoothSlowmo(0.3f, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(SmoothSlowmo(1f, 0.2f));
        if (exitApp) {
            QuitGame();
        } else if (freeplay) {
            LevelManager.instance.LoadFreeplay();
        }
        else {
            LevelManager.instance.LoadLevel(levelIndex);
        }
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    IEnumerator SmoothSlowmo(float target, float duration){
        float start = Time.timeScale;
        float t = 0;
        while (t < duration){
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        Time.timeScale = target;
    }
}
