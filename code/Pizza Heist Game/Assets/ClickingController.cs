using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickingController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //play sound when mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
        }
        
    }
}
