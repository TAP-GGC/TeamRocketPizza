using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ransomware : Virus
{
    public int taxAmount;
    // Start is called before the first frame update
    public override void UseAbilties()
    {
        if(waypointIndex == LevelManager.main.waypoints.Length){
            if(LevelManager.main.coins >= 0){
                LevelManager.main.coins -= taxAmount;
                if(LevelManager.main.coins <= 0)
                {
                   LevelManager.main.coins =0; 
                }
                
            }
            
        }
    }

    new void Update(){
        base.Update();
        UseAbilties();
    }
}
