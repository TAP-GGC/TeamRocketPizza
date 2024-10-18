using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NetworkTower : Defense
{
    
    private ParticleSystem partSys;
    public int shockDamage;

    private void Start(){
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
            fireCooldown += Time.deltaTime;
            if(fireCooldown >= 1f/firerate){
                Shoot();
                fireCooldown = 0f;
                
            }
        }
    }

    public void Shockpulse(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetRange,enemyMask);
        foreach(Collider2D c in colliders){
            Virus virus = c.GetComponent<Virus>();

            if(virus != null){
                Debug.Log("Found Virus collider");
                virus.TakeDamage(shockDamage);
                
            }
        }
    }
    public override void Shoot()
    {
        partSys.Play();
        Shockpulse();

    }
}

