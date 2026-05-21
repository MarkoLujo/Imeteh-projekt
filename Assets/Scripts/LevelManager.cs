using System;
using UnityEditor.ShaderGraph;
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
        //CreateUI();
        LoadFreeplay();
    }

    public Level[] levels;
    public GameObject[] freeplayBalls;
    public GameObject[] freeplayBaskets;
    public BallCreateAndReset ballManager;
    public HoopSetAndCreate hoopManager;
    public SceneStats sceneStats;

    //public GameObject mainUIPrefab;
    //private GameObject mainUIObject;

    public GameObject levelSelectBasketPrefabs;
    public GameObject levelSelectBaskets;

    //MainUIControl UIcontrol;

    public GameObject buttonTemplate;

    public bool isFreeplay;
    public int currentLevelIndex;
    /*
    public void uiButtonClick(int index) { 
        if (UIcontrol.levelUI.activeSelf) {

            // Level se može zapoèeti samo ako koš postoji i ako se trenutno ne mijenja
            if (hoopManager.spawnedBasket != null && !hoopManager.isPlacing) { 
                LoadLevel(index);
            }

        }
        else if (UIcontrol.ballUI.activeSelf) { 
            ballManager.ballPrefab = freeplayBalls[index];
            ballManager.DeleteBall();
        }
        else{ // Basket
            hoopManager.basketPrefab = freeplayBaskets[index];
            hoopManager.UpdateBasket();
        }
    }*/

    private void Update(){
        bool startPressed = OVRInput.GetDown(OVRInput.Button.Start);

        if (startPressed && isFreeplay) {
            if (levelSelectBaskets != null) { 
                HideUI();
            }
            else{
                ShowUI();
            }
        }
    }

    public void LoadLevel(int index) {
        isFreeplay = false;
        currentLevelIndex = index;
        HideUI();


        Level newLevel = levels[index];
        ballManager.ballPrefab = newLevel.ballPrefab;
        ballManager.DeleteBall();

        hoopManager.basketPrefab = newLevel.basketPrefab;
        hoopManager.UpdateBasket();

        sceneStats.ResetTimer();

    }

    public void ShowUI() {
        levelSelectBaskets = Instantiate(levelSelectBasketPrefabs, GameObject.FindGameObjectWithTag("Player").transform);
        levelSelectBaskets.transform.position += new Vector3(-1,1,0);
        levelSelectBaskets.transform.SetParent(null);

    }
    public void HideUI() {
        Destroy(levelSelectBaskets);
    }

    public void LoadFreeplay() {
        isFreeplay = true;
        //UIcontrol.showMainUI();

    }

    /*
    public void CreateUI() { 
        mainUIObject = Instantiate(mainUIPrefab);
        UIcontrol = mainUIObject.GetComponent<MainUIControl>();
        UIcontrol.showMainUI();

        // TODO dodat neke UI slike i/ili opis lopta, levela i koševa na gumbima

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
    */

    public void OnScore() {
        
        if (!isFreeplay) { 
            Level currentLevel = levels[currentLevelIndex];

            // Ako je level gotov
            if (sceneStats.points >= currentLevel.goal.score && (sceneStats.timer <= currentLevel.goal.timeLimit || currentLevel.goal.timeLimit <= 0)) {
                // TODO neka animacija

                if (currentLevelIndex < levels.Length-1) { 
                    currentLevelIndex++;
                    LoadLevel(currentLevelIndex);
                }
                else{
                    LoadFreeplay();
                }
            
            }
        
        
        
        }
    
    
    }


}
