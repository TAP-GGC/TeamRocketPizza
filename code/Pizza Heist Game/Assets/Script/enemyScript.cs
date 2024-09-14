using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the character

    void Update()
    {
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal"); // Default Unity axis for A and D keys
        float moveY = Input.GetAxis("Vertical"); // Default Unity axis for W and S keys
        
        Vector3 movement = new Vector3(moveX, moveY, 0f) * speed * Time.deltaTime;
        
        // Move the character
        transform.Translate(movement);
    }
}
