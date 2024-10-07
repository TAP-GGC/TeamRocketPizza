using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Virus
{
    // Start is called before the first frame update
    public override void UseAbilties()
    {
        Debug.Log("Duplicate into 2");
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(IsDead()){
            UseAbilties();
        }
    }
}
