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
