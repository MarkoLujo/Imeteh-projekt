using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BasketLowerDetectorMenu : MonoBehaviour
{
    public string nextSceneName = "NextScene";
    private bool scored = false;
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
        SceneManager.LoadScene(nextSceneName);
    }
}
