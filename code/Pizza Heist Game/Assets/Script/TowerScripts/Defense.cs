using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Defense : MonoBehaviour
{
    [Header("References")]
    public Transform rotPoint;
    public LayerMask enemyMask;
    public GameObject projectilePrefab;
    public Transform firingPoint;

    [Header("Attribute")]
    public float firerate;
    public float rotationSpeed;
    public float targetRange;
    public int cost;
    public int number;
    public Transform target;
    public float fireCooldown;

    public bool isColliding;

    public void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
        transform.position,
        targetRange,
        (Vector2)transform.position,
        0f,enemyMask);

        if(hits.Length > 0){
            target = hits[0].transform;
        }
    }
    public void Shoot(){
        GameObject projectileObj = Instantiate(projectilePrefab,firingPoint.position,Quaternion.identity);
        ProjectileController projectileScript = projectileObj.GetComponent<ProjectileController>();
        projectileScript.SetTarget(target);
    }
    public bool CheckTargetInRange(){ // Checks if the distance from target to the Turret within the range
        return Vector2.Distance(target.position, transform.position) <= targetRange;
    }

    public void RotateTowardsTarget(){
        float angle = Mathf.Atan2( // Algorithm to track angle rotation towards target
        target.position.y - transform.position.y,
        target.position.x - transform.position.x) 
        * Mathf.Rad2Deg - 90f;

        // Convert to Quaternion
        // Only make the rotation happen on the z-axis
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle)); 
        rotPoint.rotation = Quaternion.RotateTowards(rotPoint.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    public void OnDrawGizmosSelected(){ // draws a circle trigger range
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position,transform.forward,targetRange);
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if(otherGO.CompareTag("Turret")){
            Debug.Log($"Collided with: {otherGO.name}, Tag: {otherGO.tag}");
            isColliding = true;
        }
        else{
            Debug.Log("No Collision");
            isColliding = false;
        }
    }
}


