using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attribute")]
    [SerializeField] private float projSpeed = 6f;

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate(){
        if(!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * projSpeed;
    }

    public void SetTarget(Transform _target){
        target = _target;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    
}
