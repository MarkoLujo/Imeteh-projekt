using UnityEngine;

public class MainButtonScript : MonoBehaviour
{
    public int index;
    public LevelManager manager;

    public void Click() { 
        manager.uiButtonClick(index);
    }
}
