using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FirewallTower : Defense
{
    
    private ParticleSystem partSys;
    private new AudioSource audio;
    private void Start(){
        partSys = GetComponentInChildren<ParticleSystem>();
        audio = GetComponent<AudioSource>();
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

    public void Firepulse(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetRange);
        foreach(Collider2D c in colliders){
            Virus virus = c.GetComponent<Virus>();

            if(virus != null){
                Debug.Log("Found Virus collider");
                virus.ApplySlow(0.5f,3f);
                
            }
        }
    }
    public override void Shoot()
    {
        partSys.Play();
        audio.Play();
        Firepulse();

    }
}

