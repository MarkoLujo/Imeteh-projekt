using System;
using System.Collections;
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


    public Level[] levels;
    public GameObject[] freeplayBalls;
    public GameObject[] freeplayBaskets;
    public BallCreateAndReset ballManager;
    public HoopSetAndCreate hoopManager;
    public SceneStats sceneStats;



    public GameObject levelSelectBasketPrefabs;
    public GameObject levelSelectBaskets;

    public GameObject trashPrefab;
    public GameObject trash;


    public GameObject popupPrefab;
    public GameObject popupObject;
    public float popupTime = 0;
    public bool permanentPopup = false;
    Vector3 popupFinalScale;
    Vector3 popupFront;

    public GameObject mainCamera;
    public OVRCameraRig mainCamRig;


    public GameObject mainUIPrefab;
    private GameObject mainUIObject;
    MainUIControl UIcontrol;
    public GameObject buttonTemplate;


    public bool isFreeplay;
    public int currentLevelIndex;
    public bool uiActive = false;


    private void Start(){
        instance = this;
        mainCamera = GameObject.FindGameObjectWithTag("Player");
        mainCamRig = mainCamera.GetComponent<OVRCameraRig>();
        LoadFreeplay();

        StartCoroutine(ShowStartPopups());
    }

    private IEnumerator ShowStartPopups() { 

        yield return new WaitForSeconds(6);
        if (hoopManager.spawnedBasket == null) {
            ShowPopup("Pritisni [B] za postavljanje koša!", 0.6f, true);
        }
        while (hoopManager.spawnedBasket == null) { 
            yield return new WaitForSeconds(0.5f);
        }
        permanentPopup = false;

        yield return new WaitForSeconds(4);
        if (ballManager.currentBall == null) {
            ShowPopup("Pritisni [A] za dobivanje lopte", 0.6f, true);
        }
        while (ballManager.currentBall == null) { 
            yield return new WaitForSeconds(0.5f);
        }
        permanentPopup = false;

        yield return new WaitForSeconds(4);
        if (mainUIObject == null){
            ShowPopup("Za prikaz menija i levela pritisni postavke na lijevom kontroleru", 0.6f, true);
        }
        while (mainUIObject == null) { 
            yield return new WaitForSeconds(0.5f);
        }
        permanentPopup = false;
    }

    public void ShowPopup(string text, float time, bool permanent) {
        Destroy(popupObject);
        Transform eyePos = mainCamRig.centerEyeAnchor;
        popupObject = Instantiate(popupPrefab, eyePos.position, eyePos.rotation);
        popupObject.transform.eulerAngles = new Vector3(20, popupObject.transform.eulerAngles.y, 0);
        popupFront = eyePos.forward * 0.6f + eyePos.up * -0.25f;
        popupObject.transform.position += popupFront;

        popupObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        popupTime = time;
        popupFinalScale = popupObject.transform.localScale;
        popupObject.transform.localScale *= 0.0f;

        permanentPopup = permanent;
    }



    public void uiButtonClick(int index) {

        if (UIcontrol.ballUI.activeSelf) { 
            ballManager.ballPrefab = freeplayBalls[index];
            ballManager.DeleteBall();
        }
        else{ // Basket
            hoopManager.basketPrefab = freeplayBaskets[index];
            hoopManager.UpdateBasket();
        }
    }

    private void Update(){
        bool startPressed = OVRInput.GetDown(OVRInput.Button.Start);

        if (startPressed) {
            if (uiActive) { 
                //ShowPopup("Test popup", 5);
                HideUI();
            }
            else{
                ShowUI();
            }
        }

        if (popupObject != null)
        {
            if (popupTime <= 0){
                Destroy(popupObject);
            }
            else {
                // Todo možda neka više fancy animacija
                if (popupTime <= 0.6 && !permanentPopup) {
                    popupObject.transform.localScale *= 0.91f;
                }
                else {
                    popupObject.transform.localScale = popupObject.transform.localScale * 0.85f + popupFinalScale * 0.15f;
                }
                if(!permanentPopup) popupTime -= Time.deltaTime;

                // Da se pomiče zajedno s kamerom
                Transform centerEyePos = mainCamera.GetComponent<OVRCameraRig>().centerEyeAnchor;
                popupObject.transform.position = popupObject.transform.position * 0.75f + (centerEyePos.position + popupFront) * 0.25f;

                
            }
        }

    }



    public void LoadLevel(int index) {

        if (hoopManager.spawnedBasket != null && !hoopManager.isPlacing) { 
            isFreeplay = false;
            currentLevelIndex = index;
            HideUI();


            Level newLevel = levels[index];
            ballManager.ballPrefab = newLevel.ballPrefab;
            ballManager.DeleteBall();

            hoopManager.basketPrefab = newLevel.basketPrefab;
            hoopManager.UpdateBasket();

            sceneStats.ResetTimer();
            sceneStats.ResetScore();
        }


    }

    public void ShowUI() {
        HideUI();

        Transform eyePos = mainCamRig.centerEyeAnchor;

        if (isFreeplay)
        {
            if (hoopManager.spawnedBasket != null && !hoopManager.isPlacing)
            {
                levelSelectBaskets = Instantiate(levelSelectBasketPrefabs, eyePos);
                //levelSelectBaskets.transform.position += new Vector3(-1,1,0);
                levelSelectBaskets.transform.SetParent(null);
                levelSelectBaskets.transform.position = new Vector3(levelSelectBaskets.transform.position.x, 0, levelSelectBaskets.transform.position.z);
                levelSelectBaskets.transform.eulerAngles = new Vector3(0, levelSelectBaskets.transform.eulerAngles.y, 0);
            }
            else {
                ShowPopup("Prvo postavi koš za pristup levelima!", 5, false);
            }


            trash = Instantiate(trashPrefab, eyePos);
            trash.transform.SetParent(null);
            trash.transform.position = new Vector3(trash.transform.position.x, 0, trash.transform.position.z);
            trash.transform.eulerAngles = new Vector3(0, trash.transform.eulerAngles.y, 0);

            trash.transform.GetChild(1).GetComponent<BasketLowerDetectorLoadLevel>().exitApp = true;
            trash.transform.GetChild(1).GetComponent<BasketLowerDetectorLoadLevel>().freeplay = false;

            // Todo nekako napravit da se one zrake iz kontrolera pojavljuju samo kad je ui aktivan
            mainUIObject = Instantiate(mainUIPrefab, eyePos.position, eyePos.rotation);
            mainUIObject.transform.eulerAngles = new Vector3(0, mainUIObject.transform.eulerAngles.y, 0);
            mainUIObject.transform.position += eyePos.right * -0.6f + eyePos.up * -0.1f;
            mainUIObject.transform.Rotate(Vector3.up, -90);

            UIcontrol = mainUIObject.GetComponent<MainUIControl>();
            UIcontrol.showMainUI();

            // TODO dodat neke UI slike i/ili opis lopta, levela i koševa na gumbima
            for (int i = 0; i < freeplayBalls.Length; i++)
            {
                GameObject newButton = Instantiate(buttonTemplate, UIcontrol.ballUI.transform);
                MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
                buttonScript.index = i;
                buttonScript.manager = this;
            }

            for (int i = 0; i < freeplayBaskets.Length; i++)
            {
                GameObject newButton = Instantiate(buttonTemplate, UIcontrol.basketUI.transform);
                MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
                buttonScript.index = i;
                buttonScript.manager = this;
            }
        }
        else {
            trash = Instantiate(trashPrefab, eyePos);
            trash.transform.localPosition = new Vector3(0,0,0.5f);
            trash.transform.Rotate(Vector3.up, -30f);
            trash.transform.SetParent(null);
            trash.transform.position = new Vector3(trash.transform.position.x, 0, trash.transform.position.z);
            trash.transform.eulerAngles = new Vector3(0, trash.transform.eulerAngles.y, 0);


        }

        uiActive = true;
    }
    public void HideUI() {
        Destroy(levelSelectBaskets);
        Destroy(trash);
        Destroy(mainUIObject);
        uiActive = false;
    }

    public void LoadFreeplay() {
        isFreeplay = true;
        HideUI();

        ballManager.ballPrefab = freeplayBalls[0];
        ballManager.DeleteBall();

        hoopManager.basketPrefab = freeplayBaskets[0];
        hoopManager.UpdateBasket();
    }


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
