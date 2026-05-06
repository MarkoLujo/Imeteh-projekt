using UnityEngine;

public class spawnLopta : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject ballPrefab;   // ovde povuci tvoj prefab lopte
    public Transform spawnPoint;    // gde ?e se lopta pojaviti

    void Start()
    {
        // Poziva spawn nakon 10 sekundi
        Invoke("SpawnBall", 10f);
    }

    void SpawnBall()
    {
        if (ballPrefab != null && spawnPoint != null)
        {
            Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("BallPrefab ili spawnPoint nije postavljen!");
        }
    }
}
