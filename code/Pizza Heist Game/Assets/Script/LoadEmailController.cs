using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadEmailController : MonoBehaviour
{
    // Start is called before the first frame update
    //Json file that holds all the emails
    public TextAsset emailsJson;
    public GameObject emailObjectPrefab;
    public GameObject emailListPane;

    public List<Email> emails = new List<Email>();

    Email currentEmail;


[System.Serializable]
public class EmailList
{
    public List<Email> emails;
}


public EmailList emailList = new EmailList();
    void Start()
    {
        //Load all the emails from the json file
        LoadEmailsFromJson();
        LoadEmailObjectstoList();
        
    }

    // Update is called once per frame
    void Update()
    {

        //Check if the user has clicked on an email
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            //Check if the user has clicked on an email
            if (hit.collider != null)
            {   
                Debug.Log("Email Clicked");
                //Get the email object that was clicked
                GameObject emailObject = hit.collider.gameObject;

                GameObject emailListItem = emailObject.transform.Find("EmailList Item(Clone)").GetComponent<GameObject>();

                //Get the email object's subject and sender
                Text subjectText = emailObject.transform.Find("Subject").GetComponent<Text>();
                Text senderText = emailObject.transform.Find("Sender").GetComponent<Text>();

                Debug.Log("Subject: " + subjectText.text + "| Sender: " + senderText.text);

                // //Get the email object's subject and sender
                // string subject = subjectText.text;
                // string sender = senderText.text;

                // //Find the email object that was clicked
                // foreach (Email email in emails)
                // {
                //     if (email.Subject.Text == subject && email.SenderEmail.Text == sender)
                //     {
                //         currentEmail = email;
                //         break;
                //     }
                // }

                //Open the email
                //OpenEmail();
            }
        }



        
    }

    void LoadEmailsFromJson()
    {
        // Load the JSON file from Resources
        string json = emailsJson.text;

        // Deserialize the JSON into the EmailList wrapper class
        EmailList emailListWrapper = JsonUtility.FromJson<EmailList>(json);

        // Store the list of emails
        if (emailListWrapper != null)
        {
            emails = emailListWrapper.emails;
        }
        else
        {
            Debug.LogError("Failed to load emails from JSON");
        }




    }


    public void LoadEmailObjectstoList()
    {
    

        //Create a new email object for each email in the list
        foreach (Email email in emails)
        {
            GameObject emailObject = Instantiate(emailObjectPrefab, emailListPane.transform);

            // string emailSubject = email.Subject.Text;
            // string emailSender = email.SenderEmail.Text;

            // Get the TextMeshProUGUI components within the prefab
            // TextMeshProUGUI subjectText = emailObject.transform.Find("Subject").GetComponent<TextMeshProUGUI>();
            // TextMeshProUGUI senderText = emailObject.transform.Find("Sender").GetComponent<TextMeshProUGUI>();

            Text senderText = emailObject.transform.Find("Sender").GetComponent<Text>();
            Text subjectText = emailObject.transform.Find("Subject").GetComponent<Text>();

            // Set the subject and sender text from the Email object
            if (subjectText != null)
            {
                subjectText.text = email.Subject.Text;
            }
            if (senderText != null)
            {
                senderText.text = email.SenderEmail.Text;
            }
        }
    }


}
