using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Waypoints : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    Transform[] waypoints;

    int waypointIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
    if (waypointIndex < waypoints.Length) {
        Vector3 diff = waypoints[waypointIndex].position-transform.position;
        transform.right = diff;
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if(diff.magnitude < .1f) {
            waypointIndex++;
        }
    } else {
        Destroy(gameObject);
    }

    }
}
