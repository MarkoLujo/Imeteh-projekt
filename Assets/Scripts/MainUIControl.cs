using UnityEngine;

public class MainUIControl : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject levelUI;
    public GameObject ballUI;
    public GameObject basketUI;

    public void showMainUI() {
        mainUI.SetActive(true);
        levelUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
    
    }
    
    public void showLevelUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(true);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
    
    }

    
    public void showBallUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
        ballUI.SetActive(true);
        basketUI.SetActive(false);
    
    }

    
    public void showBasketUI() {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(true);
    
    }
}
