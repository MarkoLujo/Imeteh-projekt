using UnityEngine;
using TMPro;

public class LevelSwitcher: MonoBehaviour
{
    public BasketLowerDetectorLoadLevel[] levelColl; 
    public TextMeshProUGUI[] levelName;
    public GameObject[] levelBlock;

    private int currentPage = 0;

    void Start() {
        UpdateLevels();
    }

    public void GoRight() {
        if (currentPage < 1) {
            currentPage++;
            UpdateLevels();
        }
    }

    public void GoLeft() {
        if (currentPage > 0) {
            currentPage--;
            UpdateLevels();
        }
    }

    void UpdateLevels() {
        int baseLevel = currentPage * 3;

        for (int i = 0; i < 3; i++) {
            int levelNumber = baseLevel + i;
            if(levelColl!=null && levelName!=null){
            //levelName[i].text = "Level " + (levelNumber + 1);
            levelName[i].text = LevelManager.instance.levels[levelNumber].title;
            levelColl[i].levelIndex = levelNumber;
            if (LevelManager.instance.levels[levelNumber].locked){
                    levelBlock[i].SetActive(true);
                }
            else{
                    levelBlock[i].SetActive(false);
                }
            }
        }
    }
}