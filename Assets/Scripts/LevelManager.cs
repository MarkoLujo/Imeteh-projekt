using Oculus.Interaction;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public string subtitle;
    public bool locked;
}

[System.Serializable]
public struct FreeplayObject { 
    public GameObject gameObject;
    public string name;
    public Texture2D preview;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;


    public Level[] levels;
    public FreeplayObject[] freeplayBalls;
    public FreeplayObject[] freeplayBaskets;
    public BallCreateAndReset ballManager;
    public HoopSetAndCreate hoopManager;
    public SceneStats sceneStats;

    public int levelAt = 0;


    public GameObject levelSelectBasketPrefabs;
    public GameObject levelSelectBaskets;
    Vector3 levelSelectPos;
    Quaternion levelSelectRot;
    Vector3 levelSelectFwd;


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
    bool uiSpawnedAtLeastOnce = false;
    MainUIControl UIcontrol;
    public GameObject buttonTemplate;

    public GameObject titleUIPrefab;
    private GameObject titleUIObject;

    public bool isFreeplay;
    public int currentLevelIndex;
    public bool uiActive = false;


    private void Start(){
        instance = this;
        mainCamera = GameObject.FindGameObjectWithTag("Player");
        mainCamRig = mainCamera.GetComponent<OVRCameraRig>();
        uiSpawnedAtLeastOnce = false;
        LoadFreeplay();

        StartCoroutine(ShowLevelTitle("Xtreme", "basketball", 3));
        StartCoroutine(ShowStartPopups());
    }

    private IEnumerator ShowStartPopups() { 

        yield return new WaitForSeconds(6);
        if (hoopManager.spawnedBasket == null) {
            ShowPopup("Press [B] to place the basket!", 0.6f, true);
        }
        while (hoopManager.spawnedBasket == null) { 
            yield return new WaitForSeconds(0.5f);
        }
        permanentPopup = false;

        yield return new WaitForSeconds(2.7f);
        if (ballManager.currentBall == null) {
            ShowPopup("Press [A] to summon the ball", 0.6f, true);
        }
        while (ballManager.currentBall == null) { 
            yield return new WaitForSeconds(0.5f);
        }
        permanentPopup = false;

        yield return new WaitForSeconds(4);
        if (!uiSpawnedAtLeastOnce){
            ShowPopup("Press settings to show the level menu & freeplay options", 0.6f, true);
        }
        while (!uiSpawnedAtLeastOnce) { 
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
            ballManager.ballPrefab = freeplayBalls[index].gameObject;
            ballManager.DeleteBall();
        }
        else{ // Basket
            hoopManager.basketPrefab = freeplayBaskets[index].gameObject;
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

        Level currentLevel = levels[currentLevelIndex];
        if (sceneStats.timer > currentLevel.goal.timeLimit && currentLevel.goal.timeLimit > 0) {
            StartCoroutine(ShowLevelTitle("Out of time...", "", 2f));
            LoadLevelDelay();
            sceneStats.timer = -100000;
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

        if (titleUIObject != null) {
            Transform centerEyePos = mainCamera.GetComponent<OVRCameraRig>().centerEyeAnchor;
            titleUIObject.transform.position = titleUIObject.transform.position * 0.75f + (centerEyePos.position + centerEyePos.forward * 0.9f) * 0.25f;
            titleUIObject.transform.rotation = Quaternion.Slerp(titleUIObject.transform.rotation, centerEyePos.rotation, 0.25f);


        }

    }

    IEnumerator ShowLevelTitle(string titleString, string subTitleString, float duration) {
        Destroy(titleUIObject);
        Transform centerEyePos = mainCamera.GetComponent<OVRCameraRig>().centerEyeAnchor;
        titleUIObject = Instantiate(titleUIPrefab, centerEyePos.position + centerEyePos.forward * 0.7f, centerEyePos.rotation);

        Transform titleText = titleUIObject.transform.GetChild(0);
        Transform subtitleText = titleUIObject.transform.GetChild(1);

        titleText.GetComponent<TextMeshProUGUI>().text = titleString;
        subtitleText.GetComponent<TextMeshProUGUI>().text = subTitleString;


        titleText.localPosition = new Vector3(1200, titleText.localPosition.y, titleText.localPosition.z);
        subtitleText.localPosition = new Vector3(-1200, subtitleText.localPosition.y, subtitleText.localPosition.z);

        for (int i = 0; i < 75; i++) {
            titleText.localPosition += Vector3.left * 16f;
            subtitleText.localPosition += Vector3.right * 16f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < 75; i++)
        {
            titleText.localPosition += Vector3.left * 16f;
            subtitleText.localPosition += Vector3.right * 16f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(titleUIObject);

    }

    public void LoadLevel(int index) {


        if (hoopManager.spawnedBasket != null && !hoopManager.isPlacing) { 
            
            isFreeplay = false;
            currentLevelIndex = index;
            HideUI();

            StartCoroutine(ShowLevelTitle(levels[currentLevelIndex].title, levels[currentLevelIndex].subtitle, 5));

            Level newLevel = levels[index];
            ballManager.ballPrefab = newLevel.ballPrefab;
            ballManager.DeleteBall();

            hoopManager.basketPrefab = newLevel.basketPrefab;
            hoopManager.UpdateBasket();

            sceneStats.ResetTimer();
            sceneStats.ResetScore();
        }


    }

    public void ChangeLevels(bool back) { 
        if (back) levelAt -= 3;
        else levelAt += 3;
        if (levelAt < 0) levelAt = 0;
        if (levelAt >= levels.Length) levelAt = levels.Length - 1;
        SpawnLevelSelect();
    }

    
    public void SpawnLevelSelect() { 
        Destroy(levelSelectBaskets);

        levelSelectBaskets = Instantiate(levelSelectBasketPrefabs, levelSelectPos + levelSelectFwd * 1.2f, levelSelectRot);
        levelSelectBaskets.transform.position = new Vector3(levelSelectBaskets.transform.position.x, 0, levelSelectBaskets.transform.position.z);
        levelSelectBaskets.transform.eulerAngles = new Vector3(0, levelSelectBaskets.transform.eulerAngles.y, 0);
        levelSelectBaskets.transform.Rotate(Vector3.up, -90);
    
    }

    public void ShowUI() {
        HideUI();
        uiSpawnedAtLeastOnce = true;

        Transform eyePos = mainCamRig.centerEyeAnchor;
        levelSelectPos = eyePos.position;
        levelSelectRot = eyePos.rotation;
        levelSelectFwd = eyePos.forward;

        if (isFreeplay)
        {
            if (hoopManager.spawnedBasket != null && !hoopManager.isPlacing)
            {
                SpawnLevelSelect();
            }
            else {
                ShowPopup("First place the basket to access levels!", 5, false);
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
            mainUIObject.transform.position += eyePos.right * -0.7f + eyePos.up * -0.1f;
            mainUIObject.transform.Rotate(Vector3.up, -100);

            UIcontrol = mainUIObject.GetComponent<MainUIControl>();
            UIcontrol.showMainUI();

            for (int i = 0; i < freeplayBalls.Length; i++)
            {
                GameObject newButton = Instantiate(buttonTemplate, UIcontrol.ballUI.transform);
                MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
                buttonScript.index = i;
                buttonScript.manager = this;
                newButton.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(freeplayBalls[i].preview, new Rect(0,0,512,512), new Vector2(256,256));
                newButton.transform.GetChild(1).GetComponent<Text>().text = freeplayBalls[i].name;
            }

            for (int i = 0; i < freeplayBaskets.Length; i++)
            {
                GameObject newButton = Instantiate(buttonTemplate, UIcontrol.basketUI.transform);
                MainButtonScript buttonScript = newButton.GetComponent<MainButtonScript>();
                buttonScript.index = i;
                buttonScript.manager = this;
                newButton.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(freeplayBaskets[i].preview, new Rect(0, 0, 512, 512), new Vector2(256, 256)); ;
                newButton.transform.GetChild(1).GetComponent<Text>().text = freeplayBaskets[i].name;
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

        ballManager.ballPrefab = freeplayBalls[0].gameObject;
        ballManager.DeleteBall();

        hoopManager.basketPrefab = freeplayBaskets[0].gameObject;
        hoopManager.UpdateBasket();
    }



    private IEnumerator LoadLevelDelay() {
        yield return new WaitForSeconds(2);
        LoadLevel(currentLevelIndex);
    }

    public void OnScore() {
        
        if (!isFreeplay) { 
            Level currentLevel = levels[currentLevelIndex];

            // Ako je level gotov
            if (sceneStats.points >= currentLevel.goal.score && (sceneStats.timer <= currentLevel.goal.timeLimit || currentLevel.goal.timeLimit <= 0)) {

                if (currentLevelIndex < levels.Length - 1) {
                    currentLevelIndex++;
                    levels[currentLevelIndex].locked = false;
                    StartCoroutine(LoadLevelDelay());
                }
                else {
                    LoadFreeplay();
                }

            }
        
        
        
        }
    
    
    }


}
