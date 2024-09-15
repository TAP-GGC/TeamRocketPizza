using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NetTower : MonoBehaviour
{
   [Header ("Attribute")]
    [SerializeField] private float targetRange;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float firerate; //firerate per second

    [Header ("Reference")]
    [SerializeField] private Transform rotPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;
    
    private Transform target;
    private float fireCooldown;

    private void Update(){
        if (target == null){
            FindTarget();
            return;
        }

        RotateTowardsTarget();
        if(!CheckTargetInRange()){
            target = null;
        }else{
            fireCooldown += Time.deltaTime;
            if(fireCooldown >= 1f/firerate){
                StartCoroutine(ShootBurst());
                fireCooldown = 0f;
                
            }
        }
    }

    private IEnumerator ShootBurst()
{
    for (int i = 0; i < 2; i++)
    {
        Shoot();
        // Wait for a short delay between shots
        yield return new WaitForSeconds(0.2f); // Adjust the delay as needed
    }
}

    private void Shoot(){
        GameObject projectileObj = Instantiate(projectilePrefab,firingPoint.position,Quaternion.identity);
        ProjectileController projectileScript = projectileObj.GetComponent<ProjectileController>();
        projectileScript.SetTarget(target);
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
        transform.position,
        targetRange,
        (Vector2)transform.position,
        0f,enemyMask);

        if(hits.Length > 0){
            target = hits[0].transform;
        }
    }

    private bool CheckTargetInRange(){ // Checks if the distance from target to the Turret within the range
        return Vector2.Distance(target.position, transform.position) <= targetRange;
    }

    private void RotateTowardsTarget(){
        float angle = Mathf.Atan2( // Algorithm to track angle rotation towards target
        target.position.y - transform.position.y,
        target.position.x - transform.position.x) 
        * Mathf.Rad2Deg - 90f;

        // Convert to Quaternion
        // Only make the rotation happen on the z-axis
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle)); 
        rotPoint.rotation = Quaternion.RotateTowards(rotPoint.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected(){ // draws a circle trigger range
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position,transform.forward,targetRange);
    }
}

