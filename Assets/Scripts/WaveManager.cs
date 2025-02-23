using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int waveNum;

    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int enemyType; //0 1 or 2
        public int repeat; // How many times this specific instance spawns (0 is default)
        public float delay; // How long enemy will refrain from spawning
    }

    public Enemy[] enemyArray;

    private float timeSinceLastSpawn = 0f;
    void Start()
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {
            for (int j = 0; j < enemyArray[i].repeat; i++)
            {
                StartCoroutine(SpawnEnemy(enemyArray[i]));
                
            }
            
        }
    }

    IEnumerator SpawnEnemy(Enemy enemy)
    {
        yield return new WaitForSeconds(enemy.delay);

        Debug.Log("Attempting to make an enemy");

        GameObject newEnemy = Instantiate(enemy.enemyPrefab);
        //newEnemy.GetComponent<Enemy>().enemyType = enemy.type;
        GameObject model = GameObject.Find("LevelManager").GetComponent<LevelManager>().enemyModels[enemy.enemyType];
        model.transform.parent = newEnemy.transform;
        //newEnemy.GetComponent<Enemy>().possitionArray = enemy.spawns;
    }

}
