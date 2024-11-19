using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Title: LoadEmailDetails
 * Description: This script finds and loads the email details to the EmailDetailPane GameObjects in the EmailDetailPane scene.
 * Author: Brian Ramos Cazares
 * 
 * Note:
 *    - This Scripts is not used in the current version of the game. LinkInEmail is not used in the current version of the game.
 *    - Created to test the loading of email details to the EmailDetailPane GameObjects.
 */

public class LoadEmailDetails : MonoBehaviour
{
    // Start is called before the first frame update

    //Variables to store email details
    public bool isPhishingEmail;
    public string senderName;
    public string senderEmail;
    public string subject;

    public string emailContent;
    public string linkInEmail;


    //Corresponding variables to store the email details
    public GameObject emailDetailsPrefab;




    void Start()
    {
        LoadEmailDetailstoPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadEmailDetailstoPrefab()
    {
        //Load the email details to the screen

        //Get the email details prefab
        Text senderNameText = emailDetailsPrefab.transform.Find("Sender").GetComponent<Text>(); //Find the Sender Text Component
        Text senderEmailText = emailDetailsPrefab.transform.Find("Email").GetComponent<Text>(); //Find the Email Text Component
        Text subjectText = emailDetailsPrefab.transform.Find("Subject").GetComponent<Text>(); //Find the Subject Text Component
        Text emailContentText = emailDetailsPrefab.transform.Find("Body").GetComponent<Text>(); //Find the Body Text Component
        Text linkInEmailText = emailDetailsPrefab.transform.Find("Link").GetComponent<Text>(); //Find the Link Text Component

        //Set the email details
        senderNameText.text = senderName;
        senderEmailText.text = senderEmail;
        subjectText.text = subject;
        emailContentText.text = emailContent;
        linkInEmailText.text = linkInEmail;




    }


}
