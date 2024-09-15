using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; 

    [Header("Attributes")]
    [SerializeField] private int baseEnemyCount = 8;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    private int currentEnemyWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    

    void Start()
    {   

        StartWave();
        
        
    }

    void Update()
    {   
        if(!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f/spawnInterval)){

        }
    }

    void StartWave()
    {   
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        
        
    }

    // Update is called once per frame
    

    private int EnemiesPerWave(){
        enemiesLeftToSpawn = baseEnemyCount + (currentEnemyWave * 2);
        return Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(currentEnemyWave, difficultyScalingFactor));
    }
}
