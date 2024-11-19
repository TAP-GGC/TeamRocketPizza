using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

// [BACKUP TOWER CHILD CLASS]
// Purpose: A child class that only holds function for the backup tower

public class BackupTower : Defense
{
    
    private void Update(){ // A new  update method that allows a burst fire
        ClickEvent();
        if (target == null){ // If target is null find the target and then stop loop after
            FindTarget();
            return;
        }

        RotateTowardsTarget();
        if(!CheckTargetInRange()){ // if in range of the turret
            target = null;
        }else{
            fireCooldown += Time.deltaTime;
            if(fireCooldown >= 1f/firerate){ // when turret is off cooldown
                StartCoroutine(ShootBurst());
                fireCooldown = 0f;
                
            }
        }
    }

    // Shoot a burst of bullet
    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < 3; i++) // 3 bullet burst
        {
            if(target != null){
                Shoot();
            }
            // Wait for a short delay between shots
            yield return new WaitForSeconds(0.4f); // Adjust the delay as needed
        }
    }
}

