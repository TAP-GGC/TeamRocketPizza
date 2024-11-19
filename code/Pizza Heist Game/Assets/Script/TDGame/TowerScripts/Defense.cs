using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


// [DEFENSE TOWER PARENT CLASS]
// Purpose: hold function that all Tower Defense have
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
    public bool isSold = false;
    public Transform occupiedSlot;

    
    private AudioSource audioOrig;

    private void Start(){ // get any component at the first frame
        audioOrig = GetComponent<AudioSource>();
    }
    
    // Cast a radius that helps the turret find a target
    public void FindTarget(){ 
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
        transform.position,
        targetRange,
        (Vector2)transform.position,
        0f,enemyMask);

        if(hits.Length > 0){ // If the there are more than 1 hit target
            target = hits[0].transform;
        }
    }
    
    // Shoot the instantiate game object (projectile bullet)
    public virtual void Shoot(){ 
        audioOrig.Play(); // play a shoot sound
        GameObject projectileObj = Instantiate(projectilePrefab,firingPoint.position,Quaternion.identity);
        ProjectileController projectileScript = projectileObj.GetComponent<ProjectileController>();
        projectileScript.SetTarget(target);
    }
    
    // Checks if the distance from target to the Turret within the range
    public bool CheckTargetInRange(){ 
        return Vector2.Distance(target.position, transform.position) <= targetRange;
    }

    // Rotates the turret towards the target
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

    public void SetOccupiedSlot(Transform slot){
        occupiedSlot = slot; // helper method to set the state of the slots
    }

    // 
    public void ClickEvent(){
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // gets the position of the mouse;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit detected on: " + hit.collider.gameObject.name); // checks any hit detection

                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Right-click detected on this turret"); // checks right click event on the turret
                    SellCurrentInstance(); // Call your sell method here
                }
            }
        }
    }

    // Sells that exact instance of the defense
    public void SellCurrentInstance(){
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


