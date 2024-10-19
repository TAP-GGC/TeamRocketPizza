using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{

    public Animator[] anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        StartCoroutine(waitAnimation());
    }

    private IEnumerator waitAnimation(){
        for(int i = 0; i <anim.Length; i++){
            anim[i].SetTrigger("Slide");
            
            if(i == anim.Length-1){
                yield return new WaitForSeconds(0.1f);
                anim[i].SetTrigger("Floating");
            }
            yield return new WaitForSeconds(2f);
            
        }
    }
}
