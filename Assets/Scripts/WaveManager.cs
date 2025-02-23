using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int levelNum;
    public int waveNum;
    public GameObject[] enemies; // Contains ver 1, 2, 3
    public Mesh[] enemyMeshs; // Contains the version of the enemies for the level

    [System.Serializable]
    public class Enemy
    {
        public int type; // Enemy 1, 2, or 3
        public Vector2[] spawns; // Points enemy will travel
        public int repeat; // How many times this specific instance spawns (0 is default)
        public float delay; // How long enemy will refrain from spawning
    }

    public Enemy[] enemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
