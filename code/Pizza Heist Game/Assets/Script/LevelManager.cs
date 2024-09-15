using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] waypoints;



    private void Awake()
    {
        main = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
