using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PhishingGameController : MonoBehaviour
{

    //PREFAB SETUP -------------------------------------------------------------------------------------------------------------
    public TextAsset emailsJson; // JSON file containing the emails
    public GameObject emailObjectPrefab; // Prefab for the email Inbox object
    public GameObject emailDetailsPrefab; // Prefab for the email details object
    public GameObject emailListPane; // Parent object for the email list, aka the container that will hold the email inbox objects
    public GameObject emailDetailsPane; // Parent object for the email details, aka the container that will hold the email details objects
    public GameObject gameEndPrefab; // Prefab for the game end object, aka the game over screen,
    public GameObject bossChatPrefab; // Prefab for the boss chat object, this will be used to display the boss's messages when the player guesses incorrectly
    //public GameObject bossChatPane; // Parent object for the boss chat, aka the container that will hold the boss chat objects
    public GameObject HeartPrefab; // Prefab for a life heart object
    public GameObject HeartContainer; // Parent object for the life hearts, aka the container that will hold the life heart objects
    public GameObject AnswerCorrectPrefab; // Prefab for the correct answer object

    //private TextWriter.TextWriterSingle textWriterSingle;
    //TEXTWRITER SETUP -------------------------------------------------------------------------------------------------------------
    public Text dialogueText;           // UI Text that will display the message
    public Button continueButton;       // Continue Button to proceed to the next message
    public string[] messageArray;       // Array of messages to cycle through
    private int currentMessageIndex = 0; // Tracks the current message index
    private bool isTyping = false;      // Tracks if the typewriter is currently animating
    private TextWriter.TextWriterSingle textWriterSingle;




    //GAME SETUP -------------------------------------------------------------------------------------------------------------

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
        bossChatPrefab.SetActive(false);
        displayHearts();
        AnswerCorrectPrefab.SetActive(false);
        

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

        bossChatTutorialIntro();

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
            //updateBossChatMessage(emails[emailIndex]);

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
            displayAnswerCorrect();
        }
        else if (!currentEmail.IsPhishing && button.tag == "NotPhishing")
        {
            Debug.Log("Correct! This was not a phishing email.");
            displayAnswerCorrect();
        }
        else
        {
            
            //TriggerBossChatWithExplanation(currentEmail.phishingExplanation);
            OnPlayerMistake();

            Debug.Log(currentEmail.phishingExplanation);
            removeHeart();
        }

        currentEmailDetailsObject.tag = "Phished";
        currentEmailInboxObject.tag = "Phished";

        currentEmailDetailsObject.SetActive(false);
        currentEmailInboxObject.SetActive(false);

        emailLeft--;
        Debug.Log("Number of Emails Left: " + emailLeft);
        Debug.Log("Number of Attempts Left: " + attemptsLeft);

        
    }


    void OnPlayerMistake() 
    {   
        // bossChatPrefab.SetActive(true);
        // bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>().text = currentEmail.phishingExplanation;
        // Button ContinueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();
        // ContinueButton.onClick.AddListener(() => bossChatPrefab.SetActive(false));

        messageArray = new string[] {
            currentEmail.phishingExplanation
        };

        bossChatPrefab.SetActive(true);
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>();
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinuePressed);
        dialogueText.text = "";         // Clear the dialogue box initially
        continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowNextMessage(); 


    }

    void bossChatTutorialIntro()
    {
        //Set the message array for the boss chat
        messageArray = new string[] {
            "Welcome to the Phishing Game!",
            "You will be presented with a series of emails.",
            "Some of these emails are phishing emails.",
            "Your job is to identify the phishing emails.",
            "If you guess correctly, you will be rewarded.",
            "If you guess incorrectly, you will lose a life.",
            "You have 3 lives. Good luck!"
        };

        bossChatPrefab.SetActive(true);
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>();
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinuePressed);
        dialogueText.text = "";         // Clear the dialogue box initially
        continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowNextMessage(); 

        

    }

    public void ShowNextMessage()
    {
        if (currentMessageIndex < messageArray.Length)
        {
            string message = messageArray[currentMessageIndex]; // Get the current message
            currentMessageIndex++;

            // Start typewriter effect
            textWriterSingle = TextWriter.AddWriter_Static(dialogueText, message, 0.05f, false, true);
            
            isTyping = true;  // Set typing flag
            StartCoroutine(WaitForTypewriterToFinish());
        }

    }

    private IEnumerator WaitForTypewriterToFinish()
    {
        // Wait until typewriter has finished
        while (textWriterSingle != null && textWriterSingle.IsActive())
        {
            yield return null;  // Keep waiting until typing is done
        }

        // Enable "Continue" button after typing finishes
        continueButton.gameObject.SetActive(true);
        isTyping = false;  // Typing is done
    }

    private void OnContinuePressed()
    {
        if (!isTyping)
        {
            continueButton.gameObject.SetActive(false);  // Hide button after it's clicked

            if (currentMessageIndex < messageArray.Length)
            {
                // Show the next message
                ShowNextMessage();
            }
            else
            {
                // End of messages, can add logic to close dialogue box or something else
                Debug.Log("All messages have been displayed.");
                bossChatPrefab.SetActive(false);
            }
        }
    }






    private void displayHearts()
    {
        for (int i = 0; i < attemptsLeft; i++)
        {
            GameObject heart = Instantiate(HeartPrefab, HeartContainer.transform);
        }
    }

    private void removeHeart()
    {
        if (attemptsLeft > 0)
        {
            //Grab a heart from the heart container
            GameObject heart = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1).gameObject;
            Image heartImage = heart.GetComponent<Image>();
            // MAke the heart flash from red and white for 2 seconds before destroying it
            for (int i = 0; i < 2; i++)
            {
                heartImage.color = Color.red;
                StartCoroutine(WaitForSeconds(1));
                heartImage.color = Color.white;
                StartCoroutine(WaitForSeconds(1));
            }
            //Destroy the heart
            Destroy(heart);
            
            //Destroy(HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1).gameObject);
            attemptsLeft--;
        }
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private string FlashHeart(Image heartImage)
    {
        //Flash the heart red and white for 2 seconds
        for (int i = 0; i < 2; i++)
        {
            heartImage.color = Color.red;
            StartCoroutine(WaitForSeconds(1));
            heartImage.color = Color.white;
            StartCoroutine(WaitForSeconds(1));
        }
        return "Heart Flashed";
    }

    private void displayAnswerCorrect()
    {   
        //An Array of 5 different correct answer confirmation strings
        string[] correctAnswerMessages = new string[] {
            "Correct!",
            "Good Job!",
            "You got it!",
            "Nice!",
            "Well done!",
            "Great work!",
            "You're right!",
            "You're correct!",
            "You're on fire!",
            "You're a genius!",
            "You're a pro!",
            "You're a master!",
            "You're a legend!",
            "You're a champion!",
            "Awesome! Good job!",
            "Keep Going!",
        };

        //Randomly select a message from the array
        int randomIndex = Random.Range(0, correctAnswerMessages.Length);

        //Turn on the correct answer object, the wait a few seconds and slowly fade it out
        AnswerCorrectPrefab.SetActive(true);
        //Ensure the text is visible
        AnswerCorrectPrefab.GetComponent<Text>().CrossFadeAlpha(1, 0, false);

        //Set the text of the correct answer object to the randomly selected message
        AnswerCorrectPrefab.GetComponent<Text>().text = correctAnswerMessages[randomIndex];

        StartCoroutine(WaitForSeconds(1));
        AnswerCorrectPrefab.GetComponent<Text>().CrossFadeAlpha(0, 2, false);
        

    }

    
}


    






