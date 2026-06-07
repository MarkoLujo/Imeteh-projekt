using UnityEngine;
using TMPro;

public class LevelSwitcher: MonoBehaviour
{
    public BasketLowerDetectorLoadLevel[] levelColl; 
    public TextMeshProUGUI[] levelName;

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
            int levelNumber = baseLevel + i + 1;
            if(levelColl!=null && levelName!=null){
            levelName[i].text = "Level " + levelNumber;
            levelColl[i].levelIndex = levelNumber;
            }
        }
    }
}