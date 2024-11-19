using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// [FIREWALL TOWER CHILD CLASS]
// Purpose: A child class that only holds function for the firewall tower

public class FirewallTower : Defense
{
    
    private ParticleSystem partSys;
    private new AudioSource audio;
    private void Start(){ // get game component at the start of the first frame
        partSys = GetComponentInChildren<ParticleSystem>();
        audio = GetComponent<AudioSource>();
    }

    private void Update(){ // New update method that changes the shooting and target Find
        ClickEvent();
        if (target == null){
            FindTarget();
            return;
        }

        if(!CheckTargetInRange()){
            target = null;
        }else{
            fireCooldown += Time.deltaTime;
            if(fireCooldown >= 1f/firerate){
                Shoot();
                fireCooldown = 0f;
                
            }
        }
    }

    // Firewall pusle logic to slow down enemy for a certain time.
    public void Firepulse(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetRange);
        foreach(Collider2D c in colliders){
            Virus virus = c.GetComponent<Virus>();

            if(virus != null){
                Debug.Log("Found Virus collider");
                virus.ApplySlow(0.5f,3f); // Apply slow to virus
                
            }
        }
    }
    
    // Override and perform a new shoot method
    public override void Shoot() 
    {
        partSys.Play();
        audio.Play();
        Firepulse();

    }
}

