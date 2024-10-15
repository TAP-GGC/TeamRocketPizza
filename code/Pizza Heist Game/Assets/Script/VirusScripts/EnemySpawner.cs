using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; 

    [Header("Attributes")]
    [SerializeField] private int baseEnemyCount = 8;
    [SerializeField] private float spawnInterval = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent enemyDestroy = new UnityEvent();

    private int currentEnemyWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private float eps; //enemies per second
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private Text wave;
    

    private void Awake(){
        enemyDestroy.AddListener(EnemiesDestroyed);
    }


    void Start()
    {   
        wave = GameObject.Find("WaveText").GetComponent<Text>();
        StartCoroutine(StartWave()); 
    }

    void Update()
    {   
        wave.text = "Wave: " + currentEnemyWave;

        if(!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f/eps) && enemiesLeftToSpawn > 0){
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0){
            EndWave();
        }
        if(currentEnemyWave >=20){
            LevelManager.main.WinGame();
            
        }
        if(currentEnemyWave == 4 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 8 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 12 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 16 && !isSpawning){
            LevelManager.main.Warning();
        }
    }
   

    
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        currentEnemyWave++;
        StartCoroutine(StartWave()); 
    }

    private void EnemiesDestroyed()
    {
        enemiesAlive--;
    }
    private void SpawnEnemy()
    {
        if(currentEnemyWave >= 16){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,5)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 12){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,4)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 8){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,3)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 4){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,2)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else{
            GameObject prefabSpawn = enemyPrefabs[0];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
    }

    IEnumerator StartWave()
    {   
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }


    
    // Update is called once per frame
    

    private int EnemiesPerWave(){
        enemiesLeftToSpawn = baseEnemyCount + (currentEnemyWave * 2);
        return Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(currentEnemyWave, difficultyScalingFactor));
    }
    private float EnemiesPerSecond(){
        enemiesLeftToSpawn = baseEnemyCount + (currentEnemyWave * 2);
        return Mathf.Clamp(spawnInterval * Mathf.Pow(currentEnemyWave, difficultyScalingFactor), 0,enemiesPerSecondCap);
    }
}
