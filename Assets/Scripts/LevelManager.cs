using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelNum = 1;

    [Header("Add meshes for the level's enemies")]

    public GameObject[] enemyModels;
    
    public GameObject[] waveArray;
    public float delayBetweenWaves = 1.0f;
    
    private int nextWaveIndex = 0;
    private int enemiesRemainingInWave = 0;

    // Update is called once per frame
    void Update()
    {
        if (enemiesRemainingInWave <= 0)
        {
            GameObject newWave = Instantiate(waveArray[nextWaveIndex]);
            newWave.SetActive(true);
            enemiesRemainingInWave = newWave.GetComponent<WaveManager>().enemyArray.Length;
            nextWaveIndex++;
        }
    }

    void reduceEnemyCount()
    {
        enemiesRemainingInWave--;
    }
}
