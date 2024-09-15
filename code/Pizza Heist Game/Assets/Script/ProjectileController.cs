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

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate(){
        if(!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        RotateTowardsTarget();
        
        
        rb.velocity = direction * projSpeed;
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
        target = _target;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Enemy"))
    {
        Destroy(gameObject);
    }
    }
    
}
