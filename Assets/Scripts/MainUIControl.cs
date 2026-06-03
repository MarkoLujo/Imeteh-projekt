using UnityEngine;

public class MainUIControl : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject ballUI;
    public GameObject basketUI;

    public void showMainUI() {
        mainUI.SetActive(true);
        ballUI.SetActive(false);
        basketUI.SetActive(false);
    }

    public void showBallUI() {
        mainUI.SetActive(false);
        ballUI.SetActive(true);
        basketUI.SetActive(false);
    }

    public void showBasketUI() {
        mainUI.SetActive(false);
        ballUI.SetActive(false);
        basketUI.SetActive(true);
    }

}
