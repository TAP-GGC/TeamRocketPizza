using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class BackupTower : Defense
{
    
    private void Update(){
        ClickEvent();
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
        for (int i = 0; i < 3; i++)
        {
            if(target != null){
                Shoot();
            }
            // Wait for a short delay between shots
            yield return new WaitForSeconds(0.4f); // Adjust the delay as needed
        }
    }
}

