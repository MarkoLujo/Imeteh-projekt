using UnityEngine;

public class ChangeLevels : MonoBehaviour
{
    public bool back = false;

    public void ChangeLevel() { 
        LevelManager.instance.ChangeLevels(back);
    }
}
