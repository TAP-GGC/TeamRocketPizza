using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhishingGameController : MonoBehaviour
{
    public Text feedbackText; // UI Text to display feedback
    public GameObject feedbackPanel; // Optional UI Panel to show feedback

    public TextAsset emailsJson;
    public GameObject emailObjectPrefab;
    public GameObject emailDetailsPrefab;
    public GameObject emailListPane;
    public GameObject emailDetailsPane;

    [System.Serializable]
    public class EmailList
    {
        public List<Email> emails;
    }
    public EmailList emailList = new EmailList();
    public List<Email> emails = new List<Email>();
    public GameObject[] emailListObjects;
    public GameObject[] emailDetailsObjects;

    // Reference to the current email being viewed
    private Email currentEmail;
    private GameObject currentEmailInboxObject;
    private GameObject currentEmailDetailsObject;

    // Button notPhishingButton;
    // Button phishingButton;
    

    void Start()
    {
        //Load all the emails from the json file
        LoadEmailsFromJson();
        LoadEmailObjectstoList();

        emailListObjects = GameObject.FindGameObjectsWithTag("Email Inbox Item");
        emailDetailsObjects = GameObject.FindGameObjectsWithTag("Email Details");

        addListenersToGameObjects();

        //Hide all email details objects
        foreach (GameObject emailDetailsObject in emailDetailsObjects)
        {
            emailDetailsObject.SetActive(false);
        }

    }

    void Update()
    {
        


        

        
    }







    // Method to check if an element is phishing
    void CheckPhishing(Button button)
    {
        // Check if the player's answer is correct
        if (currentEmail.IsPhishing && button.tag == "IsPhishing")
        {
            
            
            removeFromInbox(currentEmail);
        }
        else if (!currentEmail.IsPhishing && button.tag == "NotPhishing")
        {
            
            
            removeFromInbox(currentEmail);
        }
        else
        {
            // Display feedback
            ShowFeedback(currentEmail);
            removeFromInbox(currentEmail);
        }
    }

    private void ShowFeedback(Email currentEmail)
    {
        // Show the feedback panel
        //feedbackPanel.SetActive(true);

        // Display feedback text
        if (currentEmail.IsPhishing)
        {
            //feedbackText.text = "Correct! This email is a phishing email.";
            Debug.Log(currentEmail.phishingExplanation);
        }
        else
        {
            //feedbackText.text = "Correct! This email is not a phishing email.";
            Debug.Log(currentEmail.phishingExplanation);
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

            //Set the email object's email
            InboxItemController inboxItemController = emailObject.GetComponent<InboxItemController>();
            inboxItemController.SetEmail(email);


            // Set the subject and sender text from the Email object
            Text senderText = emailObject.transform.Find("Sender").GetComponent<Text>();
            Text subjectText = emailObject.transform.Find("Subject").GetComponent<Text>();



            // Set the subject and sender text from the Email object
            if (subjectText != null)
            {
                subjectText.text = email.Subject;

            }
            if (senderText != null)
            {
                senderText.text = email.SenderEmail;

            }

            //Add Email Detail Panes
            GameObject emailDetailsObject = Instantiate(emailDetailsPrefab, emailDetailsPane.transform);

            EmailDetailPaneController emailDetailPaneController = emailDetailsObject.GetComponent<EmailDetailPaneController>();
            emailDetailPaneController.SetEmail(email);
            

            //Load the email details to the screen
            Text senderNameText = emailDetailsObject.transform.Find("Sender's Name Button").Find("ED Sender").GetComponent<Text>();
            Text senderEmailText = emailDetailsObject.transform.Find("Email Button").Find("ED Email").GetComponent<Text>();
            Text subjectTextDetails = emailDetailsObject.transform.Find("Subject Button").Find("ED Subject").GetComponent<Text>();
            Text emailContentText = emailDetailsObject.transform.Find("Body Button").Find("ED Body").GetComponent<Text>();


            

            //Set the email details
            senderNameText.text = email.SenderName;
            senderEmailText.text = email.SenderEmail;
            subjectTextDetails.text = email.Subject;
            emailContentText.text = email.Body;
            
        }
        
    }

    public void addListenersToGameObjects()
    {
        foreach (GameObject emailObject in emailListObjects)
        {
            Button emailButton = emailObject.GetComponent<Button>();
            //emailButton.onClick.AddListener(() => setCurrentEmail((emailObject.GetComponent<InboxItemController>().GetEmail())));
            emailButton.onClick.AddListener(() => ShowEmailDetails(emails.IndexOf(emailObject.GetComponent<InboxItemController>().GetEmail())));
        }
    }

    public void ShowEmailDetails(int emailIndex)
    {   
        //Set All open Email Details Objects to False
        foreach (GameObject emailDetailsObject in emailDetailsObjects)
        {
            emailDetailsObject.SetActive(false);
        }
        
        //Open the Email Details Object with the emailIndex
        emailDetailsObjects[emailIndex].SetActive(true);
        currentEmailDetailsObject = emailDetailsObjects[emailIndex];
        setCurrentEmail(emails[emailIndex]);



        // phishingButton = emailDetailsObjects[emailIndex].transform.Find("IsPhishing").GetComponent<Button>();
        // //Get the button text and print it to debug
        // Text phishingButtonText = phishingButton.GetComponentInChildren<Text>();
        // Debug.Log(phishingButtonText.text);
        // notPhishingButton = emailDetailsObjects[emailIndex].transform.Find("NotPhishing").GetComponent<Button>();
        // //Get the button text and print it to debug
        // Text notPhishingButtonText = notPhishingButton.GetComponentInChildren<Text>();
        // Debug.Log(notPhishingButtonText.text);

        // AddListenersToButtons();
    }

    // private void AddListenersToButtons()
    // {
    //     EmailDetailPaneController emailDetailPaneController = currentEmailDetailsObject.GetComponent<EmailDetailPaneController>();

    //     notPhishingButton.onClick.AddListener(() => emailDetailPaneController.CheckPhishing(notPhishingButton));
    //     phishingButton.onClick.AddListener(() => emailDetailPaneController.CheckPhishing(phishingButton));
    // }

    void setCurrentEmail(Email email)
    {
        currentEmail = email;
    }

    public void removeFromInbox(Email email)
    {
        emails.Remove(email);
        Destroy(currentEmailDetailsObject);
        LoadEmailObjectstoList();
        emailListObjects = GameObject.FindGameObjectsWithTag("Email Inbox Item");
        emailDetailsObjects = GameObject.FindGameObjectsWithTag("Email Details");
        addListenersToGameObjects();
    }
    





}
