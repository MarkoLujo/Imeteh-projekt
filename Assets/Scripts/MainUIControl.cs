using UnityEngine;

public class MainUIControl : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject levelUI;
    public GameObject ballUI;
    public GameObject basketUI;
    public GameObject inLevelUI;

    public void showMainUI() {
        mainUI.SetActive(true);
        levelUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
        inLevelUI.SetActive(false);
    }

    public void showLevelUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(true);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
        inLevelUI.SetActive(false);
    }

    public void showBallUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
        ballUI.SetActive(true);
        basketUI.SetActive(false);
        inLevelUI.SetActive(false);
    }

    public void showBasketUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(true);
        inLevelUI.SetActive(false);
    }

    public void showInLeveltUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
        inLevelUI.SetActive(true);
    }

    public void RetryLevel() {
        LevelManager.instance.LoadLevel(LevelManager.instance.currentLevelIndex);
    }

    public void QuitLevel() { 
        LevelManager.instance.LoadFreeplay();
    }

}
