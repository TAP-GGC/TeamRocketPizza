using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Virus : MonoBehaviour
{

    [Header("References")]
    public Rigidbody2D rb;
    // Start is called before the first frame update
    [Header("Virus Stats")]
    public int hitPoints;
    public int coinsWorth;
    public int damage;
    public float speed;
    private int waypointIndex = 0;
    private Transform target;
    // Update is called once per frame
    [SerializeField] Animator anim;

    void Start()
    {
        target = LevelManager.main.waypoints[waypointIndex];
        //myTran = transform;
    }

    void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f){
            waypointIndex++;
            
        }

        if(waypointIndex == LevelManager.main.waypoints.Length){
            EnemySpawner.enemyDestroy.Invoke();
            Destroy(gameObject);
            dealDamage();
            return;
        } else {
            target = LevelManager.main.waypoints[waypointIndex];
        }
    }

    private void FixedUpdate() {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void TakeDamage(int dmg){
        hitPoints -= dmg;
        if(hitPoints <= 0){
            EnemySpawner.enemyDestroy.Invoke();
            LevelManager.main.IncreaseCoin(coinsWorth);
            Destroy(gameObject);
            UseAbilities();
        }
    }

    public void dealDamage(){
        LevelManager.main.decreaseHealth(damage);
    }

    public abstract void UseAbilities();


    
    
}
