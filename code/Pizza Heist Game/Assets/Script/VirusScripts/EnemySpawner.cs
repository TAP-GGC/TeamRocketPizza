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
    [SerializeField] private float timeBetweenWaves = 2f;
    [SerializeField] private float difficultyScalingFactor = 0.35f;

    [Header("Events")]
    public static UnityEvent enemyDestroy = new UnityEvent();
    [SerializeField] private Button startWaveButton;
    [SerializeField] private Color originalColor; // Original color
    [SerializeField] private Color dullColor; // Dull color

    private Image buttonImage;
    private int currentEnemyWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private float eps; //enemies per second
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private Text wave;
    




    void Start()
    {   
    
        buttonImage = startWaveButton.GetComponent<Image>();
        wave = GameObject.Find("WaveText").GetComponent<Text>();
        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);
        }
        buttonImage.color = originalColor;
        // Hide the button initially
        startWaveButton.enabled = true;
    }

    void Update()
    {   
        SetButtonState();
        wave.text = "Wave: " + currentEnemyWave;

        if(!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if(timeSinceLastSpawn >= (1f/eps) && enemiesLeftToSpawn > 0){
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastSpawn = 0f;
        }

        

        if(enemiesAlive <= 0 && enemiesLeftToSpawn <= 0){
            EndWave();
        }
        if(currentEnemyWave >=16){
            LevelManager.main.WinGame();
            
        }
        if(currentEnemyWave == 3 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 6 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 9 && !isSpawning){
            LevelManager.main.Warning();
        }
        if(currentEnemyWave == 12 && !isSpawning){
            LevelManager.main.Warning();
        }
    }
   

    
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentEnemyWave++;
        // Show the button to allow starting the next wave
        LevelManager.main.WaveComplete();
        startWaveButton.enabled = true;
    }

    private void SpawnEnemy()
    {
        if(currentEnemyWave >= 12){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,5)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 9){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,4)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 6){
            GameObject prefabSpawn = enemyPrefabs[UnityEngine.Random.Range(0,3)];
            Instantiate(prefabSpawn, LevelManager.main.startPoint.position,Quaternion.identity);
        }
        else if(currentEnemyWave >= 3){
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
        enemiesAlive = 0;
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

    private void OnStartWaveButtonClicked()
    {
        // Start the next wave
        StartCoroutine(StartWave());

        // Hide the button after starting the wave
        startWaveButton.enabled = false;
        
        
    }

    public void SetButtonState()
    {
        if(startWaveButton.enabled == false){
            buttonImage.color = dullColor;
        }
        else{
            buttonImage.color = originalColor;
        }// Change color based on state
    }
}
