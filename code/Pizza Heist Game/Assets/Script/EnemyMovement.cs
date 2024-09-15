using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] private float speed;

    //Transform myTran;
    private Transform target;
    private int waypointIndex = 0;
    void Start()
    {
        target = LevelManager.main.waypoints[waypointIndex];
        //myTran = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f){
            waypointIndex++;
            
        }

        if(waypointIndex == LevelManager.main.waypoints.Length){
            Destroy(gameObject);
            return;
        } else {
            target = LevelManager.main.waypoints[waypointIndex];
        }
    }

    private void FixedUpdate() {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
