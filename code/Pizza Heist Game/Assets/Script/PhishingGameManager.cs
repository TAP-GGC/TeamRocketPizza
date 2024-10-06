using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PhishingGameController : MonoBehaviour
{
    public Text feedbackText; // UI Text to display feedback
    public GameObject feedbackPanel; // Optional UI Panel to show feedback
    //PREFAB SETUP -------------------------------------------------------------------------------------------------------------
    public TextAsset emailsJson; // JSON file containing the emails
    public GameObject emailObjectPrefab; // Prefab for the email Inbox object
    public GameObject emailDetailsPrefab; // Prefab for the email details object
    public GameObject emailListPane; // Parent object for the email list, aka the container that will hold the email inbox objects
    public GameObject emailDetailsPane; // Parent object for the email details, aka the container that will hold the email details objects
    public GameObject gameEndPrefab; // Prefab for the game end object, aka the game over screen,
    

    [System.Serializable]
    public class EmailList
    {
        public List<Email> emails;
    }
    public EmailList emailList = new EmailList(); // Wrapper class for the list of emails
    public List<Email> emails = new List<Email>(); // List of email objects
    public GameObject[] emailListObjects; // Array of email inbox objects
    public GameObject[] emailDetailsObjects; // Array of email details objects

    // Reference to the current email being viewed
    public Email currentEmail; // The current email object
    public GameObject currentEmailInboxObject; // The related inbox entry GameObject
    public GameObject currentEmailDetailsObject; // The related email details GameObject

    public Button phishingButton;
    public Button notPhishingButton;

    //Game Over Controlls
    public int emailLeft=0; //Number of emails left in the inbox
    public int attemptsLeft = 3; //Number of attempts left to guess the email
    

    void Start()
    {   
        gameEndPrefab.SetActive(false);
        

        //Load all the emails from the json file
        LoadEmailsFromJson();
        LoadEmailObjectstoList();

        emailListObjects = GameObject.FindGameObjectsWithTag("Email Inbox Item");
        emailDetailsObjects = GameObject.FindGameObjectsWithTag("Email Details");

        addListenersToGameObjects();

        emailLeft = emailListObjects.Length;
        Debug.Log(emailLeft);

        //Hide all email details objects
        foreach (GameObject emailDetailsObject in emailDetailsObjects)
        {
            emailDetailsObject.SetActive(false);
        }

    }

    void Update()
    {
                
        if(emailLeft <= 0 && attemptsLeft > 0)
        {
            //Pause Game and Display Game Over
            Time.timeScale = 0;
            //Instantiate the Game End Object for the entire screen 
            //Win text will be Green
            gameEndPrefab.SetActive(true);
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "Good Job!!";

        }
        else if (attemptsLeft <= 0 && emailLeft > 0)
        {
            //Pause Game and Display Game Over
            Time.timeScale = 0;
            //Instantiate the Game End Object for the entire screen
            //Lose text will be Red
            gameEndPrefab.SetActive(true);
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().color = Color.red;
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "You got hacked!!";
            
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

            CreateEmailDetailsPane(email, emailObject);
            
            emailLeft++;
        }
        
    }


    // This method is called when the player clicks on an email in the inbox list
    public void CreateEmailDetailsPane(Email email, GameObject inboxEntry)
    {
        // Instantiate the email details UI
        currentEmailDetailsObject = Instantiate(emailDetailsPrefab, emailDetailsPane.transform);

        // Get the EmailDetailsController component from the instantiated prefab
        EmailDetailPaneController emailDetailsController = currentEmailDetailsObject.GetComponent<EmailDetailPaneController>();

        //Load the email details to the screen
            Text senderNameText = currentEmailDetailsObject.transform.Find("Sender's Name Button").Find("ED Sender").GetComponent<Text>();
            Text senderEmailText = currentEmailDetailsObject.transform.Find("Email Button").Find("ED Email").GetComponent<Text>();
            Text subjectTextDetails = currentEmailDetailsObject.transform.Find("Subject Button").Find("ED Subject").GetComponent<Text>();
            Text emailContentText = currentEmailDetailsObject.transform.Find("Body Button").Find("ED Body").GetComponent<Text>();

        //Set the email details
            senderNameText.text = email.SenderName;
            senderEmailText.text = email.SenderEmail;
            subjectTextDetails.text = email.Subject;
            emailContentText.text = email.Body;

        // Set up the email details and pass the inbox entry
        emailDetailsController.SetUp(email, inboxEntry);

        // Subscribe to the OnEmailProcessed event
        //emailDetailsController.OnEmailProcessed += HandleEmailProcessed;
        Debug.Log("Email Details Pane Created");
    }

    private void HandleEmailProcessed(Email email, GameObject inboxEntry, bool isCorrectGuess)
    {
        if (isCorrectGuess)
        {
            // Correct guess: Remove the email from inbox and details view
            RemoveEmail(inboxEntry, currentEmailDetailsObject);
        }
        else
        {
            
            // Incorrect guess: Show the boss's message
            ShowBossMessage(email.phishingExplanation);
            RemoveEmail(inboxEntry, currentEmailDetailsObject);
            attemptsLeft--;
        }
        emailLeft--;
        Debug.Log("Number of Emails Left: " + emailLeft);
    }

    private void RemoveEmail(GameObject inboxEntry, GameObject detailsEntry)
    {
        // Destroy the inbox entry
        if (inboxEntry != null)
        {
            Destroy(inboxEntry);
        }

        // Destroy the email details entry
        if (detailsEntry != null)
        {
            Destroy(detailsEntry );
        }
    }

    // Show the boss's message explaining why the email was/wasn't phishing
    void ShowBossMessage(string message)
    {
        Debug.Log("Boss says: " + message);
        // This message could be displayed on the UI in a text box
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
        
        //Check if the tag is Phished, if it is, do not show the email details
        if (emailListObjects[emailIndex].tag == "Phished")
        {
            return;
        }else{
            //Open the Email Details Object with the emailIndex
            emailDetailsObjects[emailIndex].SetActive(true);
            currentEmailDetailsObject = emailDetailsObjects[emailIndex];
            //Set the current email inbox object to the email at the index
            currentEmailInboxObject = emailListObjects[emailIndex];

            setCurrentEmail(emails[emailIndex]);

            //Get the phishing and not phishing buttons from the email details object
            phishingButton = emailDetailsObjects[emailIndex].transform.Find("PhishingButton").GetComponent<Button>();
            notPhishingButton = emailDetailsObjects[emailIndex].transform.Find("NotPhishingButton").GetComponent<Button>();

            //Add Listeners to the buttons
            phishingButton.onClick.AddListener(() => checkEmailPhishing(phishingButton));
            notPhishingButton.onClick.AddListener(() => checkEmailPhishing(notPhishingButton));
        }


    }



    void setCurrentEmail(Email email)
    {
        currentEmail = email;
    }

    public void checkEmailPhishing(Button button)
    {   
        //Check if the player's answer is correct
        //Then Change the email objects to 'Phished' and disable from the inbox

        bool isCorrect = currentEmail.IsPhishing;
        if (currentEmail.IsPhishing && button.tag == "IsPhishing")
        {
            Debug.Log("Correct! This was a phishing email.");
        }
        else if (!currentEmail.IsPhishing && button.tag == "NotPhishing")
        {
            Debug.Log("Correct! This was not a phishing email.");
        }
        else
        {
            Debug.Log(currentEmail.phishingExplanation);
            attemptsLeft--;
        }

        currentEmailDetailsObject.tag = "Phished";
        currentEmailInboxObject.tag = "Phished";

        currentEmailDetailsObject.SetActive(false);
        currentEmailInboxObject.SetActive(false);

        emailLeft--;
        Debug.Log("Number of Emails Left: " + emailLeft);
        Debug.Log("Number of Attempts Left: " + attemptsLeft);

        
    }


    





}
