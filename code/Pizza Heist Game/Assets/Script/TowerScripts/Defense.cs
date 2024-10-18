using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Transform target;
    public float fireCooldown;

    // public bool isColliding;
    public bool isSold = false;
    public Transform occupiedSlot;
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
    public virtual void Shoot(){
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,targetRange);
    }

    public void SetOccupiedSlot(Transform slot)
    {
        occupiedSlot = slot;
    }

    public void ClickEvent(){
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit detected on: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Right-click detected on this turret");
                    SellCurrentInstance(); // Call your sell method here
                }
            }
        }
    }
    public void SellCurrentInstance()
    {
    // Assuming currentInstance has a 'Defense' component with the cost
        
        if (gameObject != null)
        {
            int refundAmount = Mathf.FloorToInt(cost * 0.75f);
            LevelManager.main.IncreaseCoin(refundAmount);

            // Reset the slot's tag to null or unoccupied
            if (occupiedSlot != null)
            {
                occupiedSlot.tag = "Slots"; // Resetting the slot to unoccupied
            }

            // Destroy the currentInstance
            Destroy(gameObject);
            isSold = true;
            Debug.Log("Current turret sold for " + refundAmount + " coins.");
        }
    }
}


