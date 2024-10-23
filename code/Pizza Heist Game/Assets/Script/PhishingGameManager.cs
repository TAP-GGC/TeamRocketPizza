using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PhishingGameController : MonoBehaviour
{

    //PREFAB SETUP -------------------------------------------------------------------------------------------------------------
    public LevelLoader ReplayTransition;
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
    public string[] messages;       // Array of messages to cycle through
    private int currentMessageIndex = 0; // Tracks the current message index
    private bool isTyping = false;      // Tracks if the typewriter is currently animating
    private TextWriter.TextWriterSingle textWriterSingle;

    //START BUTTON FUNCTIONALITY -------------------------------------------------------------------------------------------------------------
    public Button menuButton; // computer menu button in taskbar
    public GameObject menuPanel; // computer menu panel
    public Button reloadGameButton; // reload game button in computer menu
    public Button reloadInstructionsButton; // reload instructions button in computer menu
    public Button logOutButton; // log out button in computer menu, this will open a boss chat, preventing the player from ending the game



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

    public int numPhishingEmails = 0; //Number of phishing emails in the inbox
    public int numNotPhishingEmails = 0; //Number of non-phishing emails in the inbox
    

    void Start()
    {   
        gameEndPrefab.SetActive(false);
        bossChatPrefab.SetActive(false);
        displayHearts();
        AnswerCorrectPrefab.SetActive(false);
        menuPanel.SetActive(false);
        

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

        addListenersToMenuButtons();

    }

    

    void Update()
    {
        //Count All Active Emails objects in EmailListPane
        emailLeft = GameObject.FindGameObjectsWithTag("Email Inbox Item").Length;


                
        if(emailLeft <= 0 && attemptsLeft > 0)
        {
            //Pause Game and Display Game Over
            //Instantiate the Game End Object for the entire screen 
            //Win text will be Green
            gameEndPrefab.SetActive(true);
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "Good Job!!";
            //gameEndPrefab.transform.Find("Panel").Find("ReplayButton").gameObject.SetActive(false);
        }
        else if (attemptsLeft <= 0 && emailLeft > 0)
        {
            //Pause Game and Display Game Over
            
            //Instantiate the Game End Object for the entire screen
            //Lose text will be Red
            gameEndPrefab.SetActive(true);
            //Hide Return to Menu Button
            gameEndPrefab.transform.Find("Panel").Find("ReturnButton").gameObject.SetActive(false);
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().color = Color.red;
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "You got hacked!!";
        } else if (attemptsLeft <= 0 && emailLeft <= 0){
            //Pause Game and Display Game Over
            
            //Instantiate the Game End Object for the entire screen
            //Lose text will be Red
            gameEndPrefab.SetActive(true);
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().color = Color.yellow;
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "You won and lost??!";
            


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
            countEmailTypes(emailListWrapper.emails);
            emails = emailRandomizer(emailListWrapper.emails);
            
        }
        else
        {
            Debug.LogError("Failed to load emails from JSON");
        }

    }

    private void countEmailTypes(List<Email> emails)
    {
        foreach (Email email in emails)
        {
            if (email.IsPhishing)
            {
                numPhishingEmails++;
            }
            else
            {
                numNotPhishingEmails++;
            }
        }
    }

    private List<Email> emailRandomizer(List<Email> emails)
    {
        //Randomize the emails in the list and get 10 non repeating emails
        List<Email> randomEmails = new List<Email>();
        List<int> randomIndex = new List<int>();
        int index = 0;
        while (randomEmails.Count < 10)
        {
            index = Random.Range(0, emails.Count);
            if (!randomIndex.Contains(index))
            {
                randomEmails.Add(emails[index]);
                randomIndex.Add(index);
            }
        }
        return randomEmails;

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
        minimizeEmailScrollView();

        currentEmailDetailsObject.SetActive(false);
        currentEmailInboxObject.SetActive(false);

        //emailLeft--;
        Debug.Log("Number of Emails Left: " + emailLeft);
        Debug.Log("Number of Attempts Left: " + attemptsLeft);

        
    }

    private void minimizeEmailScrollView()
    {
        //Minimize the emailListPane's Rect Transfrom's Height by 200 but do not let it go past go past 1200
        RectTransform emailListPaneRectTransform = emailListPane.GetComponent<RectTransform>();
        if (emailListPaneRectTransform.sizeDelta.y > 1200){
            emailListPaneRectTransform.sizeDelta = new Vector2(emailListPaneRectTransform.sizeDelta.x, emailListPaneRectTransform.sizeDelta.y - 200);
        }

    }

    void OnPlayerMistake() 
    {   

        bossChatPrefab.SetActive(true);
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>();
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinuePressed);
        dialogueText.text = "";         // Clear the dialogue box initially
        //continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowMessage(currentEmail.phishingExplanation); 


    }

    void bossChatTutorialIntro()
    {
        //Set the message array for the boss chat
        messages = new string[] {
            "Alright Roookie, \nlets take a moment to talk about phishing emails.",
            "Phishing is a type of cyber attack where a malicious actor sends an email that appears to be from a legitimate source.\nAll in an attempt to trick you to interact with it, like clicking on a link that looks real for example.",
            "Attackers use phishing to steal sensitive information, install malware, or gain access to your computer and do damage.\nIts important to be able to identify these kinds of attacks and avoid them.",
            "I need you to clean up our emails, some of which are phishing emails.\nYour job is to identify them and destroy them. So pay close attention to the details.",
            "On your left is the list of emails, click on an email to view its details.\n\nOnce you've identified a phishing email, click on the 'Phishing' button to mark it as phishing. I will take care of the rest.\n\nIf you think an email is not phishing, click on the 'Not Phishing' button.",
            "You have 3 attempts to guess the phishing emails correctly.\nIf you run out of attempts, you will be hacked and I am not gonna be happy.",
            "Our domain name is 'BigCaesars.com', so keep that in mind as well. Play close attention to the sender's email and the content of the email.",
            "If you get stuck, click on the 'Start' button to open the menu and I'll help you out.\n\nGood luck Rookie, I'm counting on you.",
        };

        bossChatPrefab.SetActive(true);
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>();
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinuePressed);
        dialogueText.text = "";         // Clear the dialogue box initially
        //continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowMessages(messages); 

        

    }

    public void ShowMessages(string[] messagesArray)
    {
        messages = messagesArray;  // Store the array of messages
        currentMessageIndex = 0;   // Reset index
        ShowMessage(messages[currentMessageIndex]);  // Show the first message
    }

    private void ShowMessage(string message)
    {
        // Start typewriter effect for the current message
        textWriterSingle = TextWriter.AddWriter_Static(dialogueText, message, 0.02f, false, true);
        
        isTyping = true;  // Set typing flag
        StartCoroutine(WaitForTypewriterToFinish());
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
            //continueButton.gameObject.SetActive(false);  // Hide button after it's clicked
            
            currentMessageIndex++;  // Move to the next message

            if (currentMessageIndex < messages.Length)
            {
                // Show the next message in the array
                ShowMessage(messages[currentMessageIndex]);
            }
            else
            {
                // If no more messages, end the dialogue
                // dialogueText.gameObject.SetActive(false);  // Optionally hide dialogue text
                Debug.Log("All messages finished. Dialogue ends.");
                bossChatPrefab.SetActive(false);
                
            }
        } else if(isTyping){
            textWriterSingle.WriteAllAndDestroy();
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


    void addListenersToMenuButtons()
    {
        menuButton.onClick.AddListener(() => menuPanel.SetActive(!menuPanel.activeSelf));
        reloadGameButton.onClick.AddListener(() => ReplayTransition.LoadNextLevel(SceneManager.GetActiveScene().name));
        reloadInstructionsButton.onClick.AddListener(() => bossChatTutorialIntro());
        logOutButton.onClick.AddListener(() => stopPlayerExit());
    }

    private void stopPlayerExit()
    {   
        //An Array of different stop logout messages from the boss
        string[] stopLogoutDialogues = new string[] {
            "Hold on there Rookie, you can't leave yet.",
            "You can't leave yet, we have work to do.",
            "You can't leave yet, we have emails to clean up.",
            "Not so fast Rookie.",
            "Don't even think about logging out now!",
            "Wait! You still have unfinished business.",
            "The mission isn't complete yet, Rookie.",
            "Leaving so soon? Not yet, we've got emails to review.",
            "You’re not off the hook yet, Rookie.",
            "There's still work to be done. Stay sharp!",
            "Don’t be in such a hurry, Rookie. We have tasks ahead.",
            "You can’t just leave in the middle of a mission.",
            "Before you go, make sure everything is secure.",
            "No logout for you yet. We have to finish the job.",
            "Stay put! There's still more to be done.",
            "This isn't over yet, Rookie. Stay on task.",
            "The emails aren’t clean yet. Let’s finish the job.",
            "You're not done until I say you're done.",
            "There’s still phishing lurking around. Don't go yet.",
            "You can't bail out now. We’re close to wrapping this up.",
            "You’ve got more to do. Let’s see this through.",
            "The network’s not safe yet, Rookie. Stick around.",
        };

        int randomIndex = Random.Range(0, stopLogoutDialogues.Length);




        bossChatPrefab.SetActive(true);
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>();
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinuePressed);
        dialogueText.text = "";         // Clear the dialogue box initially
        continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowMessage(stopLogoutDialogues[randomIndex]);
    }
}


    






