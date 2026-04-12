using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Quit(){
        Application.Quit();
    }
    public void Restart(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void Resume(){
        pauseMenu.SetActive(false);
    }
    public void Settings(){
        
    }

}
