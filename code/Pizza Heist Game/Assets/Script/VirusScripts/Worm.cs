using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Virus
{
    public GameObject virusPrefab; // The prefab to use for duplication
    public int numberOfDuplicates = 2; // Number of duplicates to spawn
    private static int activeDuplicates = 0; // Tracks how many duplicates are still alive
    private bool isOriginal = true; // Indicates whether this is the original Worm

    private ParticleSystem explode;

    public override void Start(){
        base.Start();
        explode = GetComponentInChildren<ParticleSystem>();
    }

    public override void UseAbilties()
    {
        Debug.Log("UseAbilities");

        // Spawn the specified number of duplicates
        for (int i = 0; i < numberOfDuplicates; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0f);;
            GameObject duplicate = Instantiate(virusPrefab, spawnPosition, Quaternion.identity);

            // Mark the duplicate as a non-original copy
            Worm duplicateWorm = duplicate.GetComponent<Worm>();
            if (duplicateWorm != null)
            {
                duplicateWorm.isOriginal = false;
                duplicateWorm.SetPathInfo(waypointIndex, target);
                activeDuplicates++;
            }
        }
    }

// Method to set the duplicate's waypoint index and target
    public void SetPathInfo(int currentWaypointIndex, Transform currentTarget)
    {
        waypointIndex = currentWaypointIndex;
        target = currentTarget;
    }
    public override void OnDeath(){
        
        if(isOriginal){
            EnemySpawner.enemyDestroy.Invoke();
            explode.Play();
            UseAbilties();
        }
        else{
            activeDuplicates--;
            
            if(activeDuplicates <= 0){
                LevelManager.main.IncreaseCoin(coinsWorth);
                Debug.Log("All duplicates are dead. Awarding coins.");
            }
        }
        
        Destroy(gameObject, 0.2f);
    } 


    protected override void Update()
    {
        base.Update();
    }
}
