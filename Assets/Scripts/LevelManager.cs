using System;
using TMPro;
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

    public GameObject trashPrefab;
    public GameObject trash;

    public GameObject scoreDisplayPrefab;
    public GameObject scoreDisplay;

    //MainUIControl UIcontrol;

    //public GameObject buttonTemplate;

    public bool isFreeplay;
    public int currentLevelIndex;
    public bool uiActive = false;
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

        if (startPressed) {
            if (uiActive) { 
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

        scoreDisplay = Instantiate(scoreDisplayPrefab, hoopManager.spawnedBasket.transform);
        sceneStats.scoreDisplay = scoreDisplay.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        sceneStats.timerDisplay = scoreDisplay.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        sceneStats.ResetTimer();
        sceneStats.ResetScore();


    }

    public void ShowUI() {
        HideUI();

        if (isFreeplay)
        {
            levelSelectBaskets = Instantiate(levelSelectBasketPrefabs, GameObject.FindGameObjectWithTag("Player").transform);
            //levelSelectBaskets.transform.position += new Vector3(-1,1,0);
            levelSelectBaskets.transform.SetParent(null);
            levelSelectBaskets.transform.position = new Vector3(levelSelectBaskets.transform.position.x, 0, levelSelectBaskets.transform.position.z);

            trash = Instantiate(trashPrefab, GameObject.FindGameObjectWithTag("Player").transform);
            trash.transform.SetParent(null);
            trash.transform.position = new Vector3(trash.transform.position.x, 0, trash.transform.position.z);


            trash.transform.GetChild(1).GetComponent<BasketLowerDetectorLoadLevel>().exitApp = true;
            trash.transform.GetChild(1).GetComponent<BasketLowerDetectorLoadLevel>().freeplay = false;
        }
        else {
            trash = Instantiate(trashPrefab, GameObject.FindGameObjectWithTag("Player").transform);
            trash.transform.SetParent(null);
            trash.transform.position = new Vector3(trash.transform.position.x, 0, trash.transform.position.z);

        }

        uiActive = true;
    }
    public void HideUI() {
        Destroy(levelSelectBaskets);
        Destroy(trash);
        uiActive = false;
    }

    public void LoadFreeplay() {
        isFreeplay = true;
        HideUI();
        Destroy(scoreDisplay);
        //UIcontrol.showMainUI();

        ballManager.ballPrefab = freeplayBalls[0];
        ballManager.DeleteBall();

        hoopManager.basketPrefab = freeplayBaskets[0];
        hoopManager.UpdateBasket();
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
