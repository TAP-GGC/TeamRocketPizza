using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Text senderNameText = emailDetailsPrefab.transform.Find("Sender").GetComponent<Text>();
        Text senderEmailText = emailDetailsPrefab.transform.Find("Email").GetComponent<Text>();
        Text subjectText = emailDetailsPrefab.transform.Find("Subject").GetComponent<Text>();
        Text emailContentText = emailDetailsPrefab.transform.Find("Body").GetComponent<Text>();
        Text linkInEmailText = emailDetailsPrefab.transform.Find("Link").GetComponent<Text>();

        //Set the email details
        senderNameText.text = senderName;
        senderEmailText.text = senderEmail;
        subjectText.text = subject;
        emailContentText.text = emailContent;
        linkInEmailText.text = linkInEmail;




    }


}
