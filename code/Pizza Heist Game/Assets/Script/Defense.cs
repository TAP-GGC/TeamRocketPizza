using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] turretPrefab;
    [SerializeField] private Transform rotPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float firerate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float targetRange;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }
}


