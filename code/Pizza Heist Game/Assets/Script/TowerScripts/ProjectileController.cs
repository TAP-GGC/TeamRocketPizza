using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform rotPoint;

    [Header("Attribute")]
    [SerializeField] private float projSpeed = 1f;
    [SerializeField] private int projDamage = 1;

    private Transform target;
    private Vector2 direction;
   
    void Start(){
        // If no target is set, move in the default forward direction
        if (target == null){
            Destroy(gameObject,0.2f); // Default forward direction of the projectile
        }
    }
    
    void Update(){
       if(target != null) 
        {
            // Calculate direction towards the target
            direction = (target.position - transform.position).normalized;
            RotateTowardsTarget();
        }

        // Continue moving in the last known direction, even if the target is destroyed
        rb.velocity = direction * projSpeed;
    }

    private void RotateTowardsTarget(){ 
        if (target == null) return; // Avoid trying to rotate if there's no target

        float angle = Mathf.Atan2( // Algorithm to track angle rotation towards target
        target.position.y - transform.position.y,
        target.position.x - transform.position.x) 
        * Mathf.Rad2Deg - 90f;

        // Convert to Quaternion
        // Only make the rotation happen on the z-axis
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle)); 
        rotPoint.rotation = Quaternion.RotateTowards(rotPoint.rotation,targetRotation,235f * Time.deltaTime);
    }

    public void SetTarget(Transform _target){
        target = _target;
        if (target != null) {
            direction = (target.position - transform.position).normalized;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Virus>().TakeDamage(projDamage);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Wall")){
            Destroy(gameObject);
        }
    }
    
}
