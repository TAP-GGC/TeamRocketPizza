using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private IDSTower IDSTower;
    public GameObject exploPref;
    private HashSet<GameObject> affectedEnemies = new HashSet<GameObject>();
    private void OnParticleCollision(GameObject other){
        if(other.CompareTag("Enemy")){
                
            
            Virus virus = other.GetComponent<Virus>();
            if(virus != null){
                virus.TakeDamage(IDSTower.laserDamage);
                GameObject ex = Instantiate(exploPref, virus.transform.position, Quaternion.identity);
                Destroy(ex,1f);
                affectedEnemies.Add(other);
            }else{
                return;
            }
            
            Debug.Log("Enemy found");
            
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        IDSTower = GetComponentInParent<IDSTower>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Clear the HashSet at the end of each frame
        affectedEnemies.Clear();
    }
}
