using UnityEngine;

[System.Serializable]
public struct Requirements { 
    public int score;
    public int timeLimit;
    // TODO možda još stvari
};

[System.Serializable]
public struct Level { 
    public GameObject basketPrefab;
    public GameObject ballPrefab;
    public Requirements goal;
    public string title;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Start(){
        instance = this;
        LoadFreeplay();
    }

    public Level[] levels;
    public GameObject[] freeplayBalls;
    public GameObject[] freeplayBaskets;
    public BallCreateAndReset ballManager;
    public HoopSetAndCreate hoopManager;
    public SceneStats sceneStats;

    public GameObject freeplayUIPrefab;
    private GameObject freeplayUIObject;
    public GameObject buttonTemplate;

    public bool isFreeplay;

    public void uiButtonClick(int index) { 
        MainUIControl UIcontrol = freeplayUIObject.GetComponent<MainUIControl>();
        if (UIcontrol.levelUI.activeSelf) { 
            LoadLevel(index);
        }
        else if (UIcontrol.ballUI.activeSelf) { 
            ballManager.ballPrefab = freeplayBalls[index];
            ballManager.DeleteBall();
        }
        else{ // Basket
            hoopManager.basketPrefab = freeplayBaskets[index];
            hoopManager.UpdateBasket();
        }
    
    
    
    }



    public void LoadLevel(int index) {
        isFreeplay = false;
        DestroyFreeplayUI();

        Level newLevel = levels[index];
        ballManager.ballPrefab = newLevel.ballPrefab;
        ballManager.DeleteBall();

        hoopManager.basketPrefab = newLevel.basketPrefab;
        hoopManager.UpdateBasket();

        sceneStats.ResetTimer();

    }

    private void DestroyFreeplayUI() { 
        if (freeplayUIObject != null)
        {
            Destroy(freeplayUIObject);
        }
    }

    public void LoadFreeplay() {
        isFreeplay = true;
        DestroyFreeplayUI();

        freeplayUIObject = Instantiate(freeplayUIPrefab);
        freeplayUIObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.left;

        MainUIControl UIcontrol = freeplayUIObject.GetComponent<MainUIControl>();

        freeplayUIObject.GetComponent<MainUIControl>().showMainUI();
        for(int i=0; i<levels.Length; i++){
            GameObject newButton = Instantiate(buttonTemplate, UIcontrol.levelUI.transform);
            MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
            buttonScript.index = i;
            buttonScript.manager = this; 
        }

        for(int i=0; i<freeplayBalls.Length; i++){
            GameObject newButton = Instantiate(buttonTemplate, UIcontrol.ballUI.transform);
            MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
            buttonScript.index = i;
            buttonScript.manager = this;        
        }


        for(int i=0; i<freeplayBaskets.Length; i++){
            GameObject newButton = Instantiate(buttonTemplate, UIcontrol.basketUI.transform);
            MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
            buttonScript.index = i;
            buttonScript.manager = this;        
        }
    }

}
