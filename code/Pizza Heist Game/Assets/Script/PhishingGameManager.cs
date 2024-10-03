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


    GameObject[] emailDetailsObjects;



    // UI Elements for the email
    public Button subjectButton;
    public Button senderNameButton;
    public Button senderEmailButton;
    public Button bodyButton;
    public Button linkButton;

    // Reference to the current email being viewed
    private Email currentEmail;

    void Start()
    {
        //Load all the emails from the json file
        LoadEmailsFromJson();
        LoadEmailObjectstoList();

        emailDetailsObjects = GameObject.FindGameObjectsWithTag("Email Details");


        // Assign click events to the buttons
        // subjectButton.onClick.AddListener(() => CheckPhishing(currentEmail.Subject));
        // senderNameButton.onClick.AddListener(() => CheckPhishing(currentEmail.SenderName));
        // senderEmailButton.onClick.AddListener(() => CheckPhishing(currentEmail.SenderEmail));
        // bodyButton.onClick.AddListener(() => CheckPhishing(currentEmail.Body));
        // linkButton.onClick.AddListener(() => CheckPhishing(currentEmail.Link));

        // Hide the feedback panel initially
        //feedbackPanel.SetActive(false);
    }

    void Update()
    {
        // Check if the user pressed the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Hide the feedback panel
            feedbackPanel.SetActive(false);
        }

        
    }







    // Method to check if an element is phishing
    void CheckPhishing(EmailElement element)
    {
        if (element.IsPhishing)
        {
            // Display positive feedback
            feedbackText.text = "Good job! You identified a phishing attempt.\nReason: " + element.Text;
        }
        else
        {
            // Display neutral feedback (optional)
            feedbackText.text = "This is not a phishing attempt, try again.";
        }

        // Show feedback
        feedbackPanel.SetActive(true);
    }

    // Call this method to load a new email (for example, when an email is clicked)
    public void LoadEmail(Email email)
    {
        currentEmail = email;

        // Set the UI text for the email (you could have separate UI Text objects for each element)
        subjectButton.GetComponentInChildren<Text>().text = email.Subject.Text;
        senderNameButton.GetComponentInChildren<Text>().text = email.SenderName.Text;
        senderEmailButton.GetComponentInChildren<Text>().text = email.SenderEmail.Text;
        bodyButton.GetComponentInChildren<Text>().text = email.Body.Text;
        linkButton.GetComponentInChildren<Text>().text = email.Link.Text;

        // Hide feedback when a new email is loaded
        feedbackPanel.SetActive(false);
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



            //Add Email Detail Panes
            GameObject emailDetailsObject = Instantiate(emailDetailsPrefab, emailDetailsPane.transform);
            

            //Load the email details to the screen
            Text senderNameText = emailDetailsObject.transform.Find("Sender's Name Button").Find("ED Sender").GetComponent<Text>();
            Text senderEmailText = emailDetailsObject.transform.Find("Email Button").Find("ED Email").GetComponent<Text>();
            Text subjectTextDetails = emailDetailsObject.transform.Find("Subject Button").Find("ED Subject").GetComponent<Text>();
            Text emailContentText = emailDetailsObject.transform.Find("Body Button").Find("ED Body").GetComponent<Text>();


            

            //Set the email details
            senderNameText.text = email.SenderName.Text;
            senderEmailText.text = email.SenderEmail.Text;
            subjectTextDetails.text = email.Subject.Text;
            emailContentText.text = email.Body.Text;

            emailDetailsObject.SetActive(false);
            

            //Set the email details
            
        }
    }





}
