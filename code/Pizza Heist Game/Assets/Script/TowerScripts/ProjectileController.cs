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
    private bool isDirectionSet = false; // Flag to ensure direction is only set once
    private bool targetDestroyed = false; // Check if target has been destroyed
    
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate(){
        if(!target) return;
        if (isDirectionSet && !targetDestroyed)
        {
            rb.velocity = direction * projSpeed;
        }
    }

    private void RotateTowardsTarget(){ 
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
        if(_target != null){
            target = _target;
            direction = (target.position - transform.position).normalized;
            RotateTowardsTarget();
            isDirectionSet = true;
        }
        else{
            // If no target is set, mark as direction is set, but do not attempt rotation
            isDirectionSet = true;
            targetDestroyed = true;

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
