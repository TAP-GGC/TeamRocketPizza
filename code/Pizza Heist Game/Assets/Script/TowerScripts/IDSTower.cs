using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IDSTower : Defense
{
    
    private ParticleSystem partSys;
    public int laserDamage;

    private void Start(){
        partSys = GetComponentInChildren<ParticleSystem>();
    }

    private void Update(){
        if (target == null){
            FindTarget();
            return;
        }
        
        if(!CheckTargetInRange()){
            target = null;
        }else{
            RotateTowardsTarget();
            fireCooldown += Time.deltaTime;
            if(fireCooldown >= 1f/firerate){
                
                Shoot();
                fireCooldown = 0f;
                
            }
        }
    }

    
    public override void Shoot()
    {
        partSys.Play();

    }
}

