using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// [IDS TOWER CHILD CLASS]
// Purpose: A child class that only holds function for the backup tower
public class IDSTower : Defense
{
    
    private ParticleSystem partSys;
    public int laserDamage;
    private new AudioSource audio;
    private void Start(){ // Get component at the start of the first frame
        audio = GetComponent<AudioSource>();
        partSys = GetComponentInChildren<ParticleSystem>();
    }

    private void Update(){ 
        ClickEvent();
        if (target == null){
            FindTarget();
            return;
        }
        
        if(!CheckTargetInRange()){
            target = null;
        }else{
            RotateTowardsTarget(); // Rotate target
            fireCooldown += Time.deltaTime; // Cooldown timer
            if(fireCooldown >= 1f/firerate){
                
                Shoot(); // Shoot 
                fireCooldown = 0f;
                
            }
        }
    }

    // Override the shoot method
    public override void Shoot() 
    {
        partSys.Play();
        audio.Play();
    }
}

