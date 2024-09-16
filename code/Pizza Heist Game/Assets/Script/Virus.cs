using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Virus Stats")]
    [SerializeField] private int hitPoints;
    [SerializeField] private int coinsWorth;
    [SerializeField] private int damage;
    // Update is called once per frame
    [SerializeField] Animator anim;

    
    public void TakeDamage(int dmg){
        hitPoints -= dmg;
        if(hitPoints <= 0){
            EnemySpawner.enemyDestroy.Invoke();
            LevelManager.main.IncreaseCoin(coinsWorth);
            Destroy(gameObject);
            
        }
    }

    public void dealDamage(){
        LevelManager.main.decreaseHealth(damage);
    }


    
    
}
