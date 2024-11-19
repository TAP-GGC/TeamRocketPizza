using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/*
 * Title: PhishingGameController
 * Description: This is the main controller for the Phishing Game. It controls the game flow and the interactions between the different game objects.
 * Author: Brian Ramos Cazares
 * 
 * Note:
 *    - This script is not ideal and needs to be refactored. It is currently doing too much and needs to be broken down into smaller scripts. It all works nontheless.
 *    - 
 */


public class PhishingGameController : MonoBehaviour
{

    //PREFAB SETUP -------------------------------------------------------------------------------------------------------------
    [Header("Prefab Setup")] //This will display a header in the inspector
    public LevelLoader ReplayTransition; // Level Loader for the game
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
    [Header("Text Writer Setup")] //This will display a header in the inspector
    public Text dialogueText;           // UI Text that will display the message
    public Button continueButton;       // Continue Button to proceed to the next message
    public string[] messages;       // Array of messages to cycle through
    private int currentMessageIndex = 0; // Tracks the current message index
    private bool isTyping = false;      // Tracks if the typewriter is currently animating
    private TextWriter.TextWriterSingle textWriterSingle;

    //START BUTTON FUNCTIONALITY -------------------------------------------------------------------------------------------------------------
    [Header("Start Button Functionality")] //This will display a header in the inspector
    public Button menuButton; // computer menu button in taskbar
    public GameObject menuPanel; // computer menu panel
    public Button reloadGameButton; // reload game button in computer menu
    public Button reloadInstructionsButton; // reload instructions button in computer menu
    public Button logOutButton; // log out button in computer menu, this will open a boss chat, preventing the player from ending the game



    //GAME SETUP -------------------------------------------------------------------------------------------------------------
    // This section contains the email setup variables as well as the email lists and objects

    [System.Serializable] // Wrapper class for the list of emails, This is necessary to load the JSON file
    public class EmailList // Wrapper class for the list of emails
    {
        public List<Email> emails; // List of email objects
    }
    public EmailList emailList = new EmailList(); // Wrapper class for the list of emails
    public List<Email> emails = new List<Email>(); // List of email objects
    public GameObject[] emailListObjects; // Array of email inbox objects
    public GameObject[] emailDetailsObjects; // Array of email details objects

    // Reference to the current email being viewed
    [Header("Current Email")]
    public Email currentEmail; // The current email object
    public GameObject currentEmailInboxObject; // The related inbox entry GameObject
    public GameObject currentEmailDetailsObject; // The related email details GameObject

    public Button phishingButton; // The Phishing button from the email details object
    public Button notPhishingButton; // The Not Phishing button from the email details object

    //Game Over Controlls
    [Header("Game Over Controls")]
    public int emailLeft=0; //Number of emails left in the inbox
    public int attemptsLeft = 3; //Number of attempts left to guess the email

    public int numPhishingEmails = 0; //Number of phishing emails in the inbox
    public int numNotPhishingEmails = 0; //Number of non-phishing emails in the inbox
    

    void Start()
    {   
        gameEndPrefab.SetActive(false); //Hide the game end ui screen object
        bossChatPrefab.SetActive(false); //Hide the boss chat ui screen object
        displayHearts(); //Display the hearts on the bottom left side of screen, next to start button
        AnswerCorrectPrefab.SetActive(false); //Hide the correct answer ui screen object in bottom right corner, This is where the "Correct!" message will appear
        menuPanel.SetActive(false); //Hide the computer menu panel
        

        //Load all the emails from the json file
        LoadEmailsFromJson(); //Methon to Load the emails from the json file
        LoadEmailObjectstoList(); //Method to Load the emails to the email list pane

        emailListObjects = GameObject.FindGameObjectsWithTag("Email Inbox Item"); //Counts all the email inbox objects in the email list pane
        emailDetailsObjects = GameObject.FindGameObjectsWithTag("Email Details"); //Counts all the email details objects in the email details pane

        addListenersToGameObjects(); //Add Listeners to the email inbox objects, to make them clickable

        emailLeft = emailListObjects.Length; //Set the number of emails left to the number of email inbox objects, this is for game over conditions
        Debug.Log(emailLeft); //Print the number of emails left to the console

        //Hide all email details objects
        foreach (GameObject emailDetailsObject in emailDetailsObjects) //Loop through all the email details objects
        {
            emailDetailsObject.SetActive(false); //Hide all the email details objects so the game starts with a blank email details pane
        }

        bossChatTutorialIntro(); //Start the boss chat tutorial intro

        addListenersToMenuButtons(); //Add Listeners to the menu buttons

    }

    

    void Update()
    {
        //Count All Active Emails objects in EmailListPane
        emailLeft = GameObject.FindGameObjectsWithTag("Email Inbox Item").Length; //Always count and update the number of emails left, for game over conditions


                
        if(emailLeft <= 0 && attemptsLeft > 0) //Good Ending: If there are no emails left and the player still has attempts/hearts left
        {
            //Pause Game and Display Game Over
            //Instantiate the Game End Object for the entire screen 
            //Win text will be Green
            gameEndPrefab.SetActive(true); //Show the game end ui screen object
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "Good Job!!"; //Set the text of the game end ui screen object to "Good Job!!" instead of "Game End"
            //gameEndPrefab.transform.Find("Panel").Find("ReplayButton").gameObject.SetActive(false);
        }
        else if (attemptsLeft <= 0 && emailLeft > 0) //Bad Ending: If the player has no attempts/hearts left and there are still emails left
        {
            //Pause Game and Display Game Over
            
            //Instantiate the Game End Object for the entire screen
            //Lose text will be Red
            gameEndPrefab.SetActive(true);
            //Hide Return to Menu Button
            gameEndPrefab.transform.Find("Panel").Find("ReturnButton").gameObject.SetActive(false); //Hide the return to menu button
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().color = Color.red; //Set the text color of the game end ui screen object to red
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "You got hacked!!"; //Set the text of the game end ui screen object to "You got hacked!!" instead of "Game End"
        } else if (attemptsLeft <= 0 && emailLeft <= 0){ //Secret Ending: If the player has no attempts/hearts left and there are no emails left
            //Pause Game and Display Game Over
            
            //Instantiate the Game End Object for the entire screen
            //Lose text will be Red
            gameEndPrefab.SetActive(true); //Show the game end ui screen object
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().color = Color.yellow; //Set the text color of the game end ui screen object to yellow
            gameEndPrefab.transform.Find("Panel").Find("GameOverText").GetComponent<Text>().text = "You won and lost??!"; //Set the text of the game end ui screen object to "You won and lost??!" instead of "Game End"
            


        }
        


        
    }



    void LoadEmailsFromJson() //Method to Load the emails from the json file
    {
        // Load the JSON file from Resources
        string json = emailsJson.text; //Load the json file as a string

        // Deserialize the JSON into the EmailList wrapper class
        EmailList emailListWrapper = JsonUtility.FromJson<EmailList>(json); //Deserialize the json file into the EmailList wrapper class

        // Store the list of emails
        if (emailListWrapper != null) //If the email list wrapper is not empty
        {
            countEmailTypes(emailListWrapper.emails); //Count the number of phishing and non-phishing emails
            emails = emailRandomizer(emailListWrapper.emails); //Randomize the emails in the list and get 10 non repeating emails
            
        }
        else
        {
            Debug.LogError("Failed to load emails from JSON"); //Print an error message to the console if the email list wrapper is empty
        }

    }

    private void countEmailTypes(List<Email> emails) //Count the number of phishing and non-phishing emails
    {
        foreach (Email email in emails) //Loop through all the emails in the list
        {
            if (email.IsPhishing) //If the email is a phishing email
            {
                numPhishingEmails++; //Increment the number of phishing emails
            }
            else //If the email is not a phishing email
            {
                numNotPhishingEmails++; //Increment the number of non-phishing emails
            }
        }
    }

    private List<Email> emailRandomizer(List<Email> emails) //Randomize the emails in the list and get 10 non repeating emails
    {
        //Randomize the emails in the list and get 10 non repeating emails
        List<Email> randomEmails = new List<Email>(); //Create a new list of emails
        List<int> randomIndex = new List<int>(); //Create a new list of random indexes
        int index = 0; //Set the index to 0
        while (randomEmails.Count < 10) //While the number of random emails is less than 10
        {
            index = Random.Range(0, emails.Count); //Get a random index between 0 and the total number of emails in the list
            if (!randomIndex.Contains(index)) //If the random index is not in the list of random indexes
            {
                randomEmails.Add(emails[index]); //Add the email at the index to the list of random emails
                randomIndex.Add(index); //Add the index to the list of random indexes
            }
        }
        return randomEmails; //Return the list of random emails

    }

    public void LoadEmailObjectstoList() //Method to create inbox objects for each email in the list, assuming the list is not empty
    {
    

        //Create a new email object for each email in the list
        foreach (Email email in emails)
        {
            GameObject emailObject = Instantiate(emailObjectPrefab, emailListPane.transform); //Instantiate the email object prefab and set the parent to the email list pane

            //Set the email object's email
            InboxItemController inboxItemController = emailObject.GetComponent<InboxItemController>(); //Get the InboxItemController component from the email object
            inboxItemController.SetEmail(email); //Set the email in the InboxItemController component


            // Set the subject and sender text from the Email object
            Text senderText = emailObject.transform.Find("Sender").GetComponent<Text>(); //Find the Sender Text Component
            Text subjectText = emailObject.transform.Find("Subject").GetComponent<Text>(); //Find the Subject Text Component



            // Set the subject and sender text from the Email object
            if (subjectText != null) 
            {
                subjectText.text = email.Subject; //Set the subject text to the email's subject if the subject text is not null/empty

            }
            if (senderText != null)
            {
                senderText.text = email.SenderEmail; //Set the sender text to the email's sender email if the sender text is not null/empty

            }

            CreateEmailDetailsPane(email, emailObject); //Create the email details pane for the current email in the loop
            
        }
        
    }


    //Create the email details pane for a given email with its corresponding inbox entry object
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

        Debug.Log("Email Details Pane Created"); //Print a message to the console
    }


    public void addListenersToGameObjects() //Add Listeners to the email inbox objects, to make them clickable
    {
        foreach (GameObject emailObject in emailListObjects) //Loop through all the email inbox objects
        {
            Button emailButton = emailObject.GetComponent<Button>(); //Get the Button component from the email inbox object

            //Add a listener to the email inbox object to show the email details with the index of the corresponding email from the list of emails from json
            emailButton.onClick.AddListener(() => ShowEmailDetails(emails.IndexOf(emailObject.GetComponent<InboxItemController>().GetEmail()))); 
        }
    }

    ////Show the email details with a given index from the list of emails from json
    public void ShowEmailDetails(int emailIndex) 
    {   
        //Set All Active, if any, Email Details Objects to False, to ensure a blank email details pane
        foreach (GameObject emailDetailsObject in emailDetailsObjects)
        {
            emailDetailsObject.SetActive(false); //Hide all the email details objects
        }
        
        //Check if the tag is Phished, if it is, do not show the email details
        //Phished is the tag given to the email inbox object when the email is completed by the player, correct or not
        if (emailListObjects[emailIndex].tag == "Phished")
        {
            return; //Exits the method if the email inbox object is already phished
        }else{

            //Open the Email Details Object with the emailIndex
            emailDetailsObjects[emailIndex].SetActive(true); //Show the email details if the player has not completed the email
            currentEmailDetailsObject = emailDetailsObjects[emailIndex]; //Sets the current email details object to the email details object at the index
            //Set the current email inbox object to the email at the index
            currentEmailInboxObject = emailListObjects[emailIndex];

            setCurrentEmail(emails[emailIndex]); //Set the current email to the email at the index
            

            //Get the phishing and not phishing buttons from the new active email details object
            phishingButton = emailDetailsObjects[emailIndex].transform.Find("PhishingButton").GetComponent<Button>(); //Find the Phishing Button Component of that email details object
            notPhishingButton = emailDetailsObjects[emailIndex].transform.Find("NotPhishingButton").GetComponent<Button>(); //Find the Not Phishing Button Component of that email details object

            //Add Listeners to the buttons newly found buttons, its the same listener. Checks if the player correctly marked email as phishing/Not Phishing or not
            phishingButton.onClick.AddListener(() => checkEmailPhishing(phishingButton)); 
            notPhishingButton.onClick.AddListener(() => checkEmailPhishing(notPhishingButton)); 
        }


    }

    

    void setCurrentEmail(Email email) //Set the current email to the email at the index
    {
        currentEmail = email; //Set the current email to the email at the index
    }


    public void checkEmailPhishing(Button button)
    {   
        //Check if the player's answer is correct
        //Then Change the email objects to 'Phished' and disable from the inbox

        bool isCorrect = currentEmail.IsPhishing; //Set the isCorrect variable to the current email's phishing status
        if (currentEmail.IsPhishing && button.tag == "IsPhishing") //If player correctly marked email as phishing
        {
            Debug.Log("Correct! This was a phishing email."); //Print a message to the console, debuggin purposes
            displayAnswerCorrect(); //Display the correct answer object in bottom right corner
        }
        else if (!currentEmail.IsPhishing && button.tag == "NotPhishing") //If player correctly marked email as not phishing
        {
            Debug.Log("Correct! This was not a phishing email."); //Print a message to the console, debuggin purposes
            displayAnswerCorrect(); //Display the correct answer object in bottom right corner
        }
        else //If player incorrectly marked email as phishing/not phishing
        {
            
            OnPlayerMistake(); //Display the boss chat object in the middle of the screen, boss will give a explanation of why the email is incorrect

            Debug.Log(currentEmail.phishingExplanation); //Print the phishing explanation to the console, debuggin purposes
            removeHeart(); //Remove a heart from the bottom left corner, also known as the attempts left
        }

        currentEmailDetailsObject.tag = "Phished"; //Set the current email details object's tag to "Phished"
        currentEmailInboxObject.tag = "Phished"; //Set the current email inbox object's tag to "Phished"
        minimizeEmailScrollView(); //Minimize the email list pane's Rect Transfrom's Height by 200 but do not let it go past go past 1200, This is update the scrollbar

        currentEmailDetailsObject.SetActive(false); //Hide the current email details object
        currentEmailInboxObject.SetActive(false); //Hide the current email inbox object
        
        Debug.Log("Number of Emails Left: " + emailLeft); //Print the number of emails left to the console, debuggin purposes
        Debug.Log("Number of Attempts Left: " + attemptsLeft); //Print the number of attempts left to the console, debuggin purposes

        
    }

    //This will adjust the inbox scroll view height to fit the number of emails left
    private void minimizeEmailScrollView() 
    {
        //Minimize the emailListPane's Rect Transfrom's Height by 200 but do not let it go past go past 1200
        RectTransform emailListPaneRectTransform = emailListPane.GetComponent<RectTransform>();
        if (emailListPaneRectTransform.sizeDelta.y > 1200){ //If the email list pane's height is greater than 1200, then minimize it by 200, otherwise do nothing
            emailListPaneRectTransform.sizeDelta = new Vector2(emailListPaneRectTransform.sizeDelta.x, emailListPaneRectTransform.sizeDelta.y - 200); //Minimize the email list pane's height by 200
        }

    }

    //This will make the boss chat object appear in the middle of the screen, and give a explanation of why the email is incorrect
    void OnPlayerMistake()  
    {   

        bossChatPrefab.SetActive(true); //Show the boss chat object
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>(); //Find the Boss Dialogue Text Component
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>(); //Find the Continue Button Component

        continueButton.onClick.AddListener(OnContinuePressed); //Add a listener to the continue button to proceed to the next message
        dialogueText.text = "";         // Clear the dialogue box initially
        ShowMessage(currentEmail.phishingExplanation);  //Show the phishing explanation of the current email, located in the email object


    }


    //This is the boss chat tutorial intro, the boss will explain the game to the player
    void bossChatTutorialIntro()
    {
        //Set the message array for the boss chat, this is the tutorial intro dialogue
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

        bossChatPrefab.SetActive(true); //Show the boss chat object
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>(); //Find the Boss Dialogue Text Component
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>(); //Find the Continue Button Component

        continueButton.onClick.AddListener(OnContinuePressed); //Add a listener to the continue button to proceed to the next message
        dialogueText.text = "";         // Clear the dialogue box initially
        ShowMessages(messages);  //Show the messages in the array

        

    }

    //This will show the messages in an ARRAY, one by one, Must be called with an array of messages
    public void ShowMessages(string[] messagesArray) 
    {
        messages = messagesArray;  // Store the array of messages
        currentMessageIndex = 0;   // Reset index
        ShowMessage(messages[currentMessageIndex]);  // Show the first message
    }

    //This will show the messages for a single line, and only one line.
    private void ShowMessage(string message)
    {
        // Start typewriter effect for the current message
        // This will show the message one character at a time
        textWriterSingle = TextWriter.AddWriter_Static(dialogueText, message, 0.02f, false, true);
        
        isTyping = true;  // Set typing flag, means typing is in progress
        StartCoroutine(WaitForTypewriterToFinish()); // Wait for typing to finish
    }

    //This will wait for the typewriter to finish, then enable the continue button
    // Player can click the button to move to the next message
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

    //This will show the next message in the array
    private void OnContinuePressed()
    {
        if (!isTyping) //If the typewriter is not currently animating
        {
            
            currentMessageIndex++;  // Move to the next message

            if (currentMessageIndex < messages.Length) //If there are more messages in the array
            {
                // Show the next message in the array
                ShowMessage(messages[currentMessageIndex]);
            }
            else //If no more messages in the array, dialogue ends, hide the boss chat object
            {
                
                Debug.Log("All messages finished. Dialogue ends."); //Print a message to the console, debuggin purposes
                bossChatPrefab.SetActive(false); //Hide the boss chat object
                
            }
        } else if(isTyping){ //If the typewriter is currently animating
            textWriterSingle.WriteAllAndDestroy(); //Show the entire message at once
        }
    }
    






    //This will count the number of attempts left, and create a heart at the bottom left corner
    private void displayHearts()
    {
        for (int i = 0; i < attemptsLeft; i++) // For each attempt left
        {
            GameObject heart = Instantiate(HeartPrefab, HeartContainer.transform); //Instantiate a heart prefab and set the parent to the heart container
        }
    }


    //Method Used to remove a heart from the bottom left corner, when a player guesses incorrectly
    private void removeHeart()
    {
        if (attemptsLeft > 0) //If the player still has attempts left
        {
            //Grab a heart from the heart container
            GameObject heart = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1).gameObject; 
            Image heartImage = heart.GetComponent<Image>();
            // MAke the heart flash from red and white for 2 seconds before destroying it
            for (int i = 0; i < 2; i++)
            {
                heartImage.color = Color.red; //Set the heart color to red
                StartCoroutine(WaitForSeconds(1)); //Wait for 1 second
                heartImage.color = Color.white; //Set the heart color to white
                StartCoroutine(WaitForSeconds(1)); //Wait for 1 second
            }
            //Destroy the heart, remove it from the corresponding area
            Destroy(heart);
            
            attemptsLeft--; //Decrement the number of attempts left,
        }
    }


    //This will wait for a given number of seconds
    IEnumerator WaitForSeconds(int seconds) 
    {
        yield return new WaitForSeconds(seconds); //Unity Coroutine to wait for a given number of seconds
    }


    //Unused
    // This will flash the heart red and white for 2 seconds
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

    //This will display a random correct answer message from a list in the bottom right corner
    private void displayAnswerCorrect()
    {   
        //An Array of different correct answer confirmation strings
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

        StartCoroutine(WaitForSeconds(1)); //Wait for 1 second
        AnswerCorrectPrefab.GetComponent<Text>().CrossFadeAlpha(0, 2, false); //Fade out the correct answer object over 2 seconds
        

    }


    //This will add listeners to the start menu buttons
    void addListenersToMenuButtons()
    {
        menuButton.onClick.AddListener(() => menuPanel.SetActive(!menuPanel.activeSelf)); //Add a listener to the menu button to show/hide the menu panel
        reloadGameButton.onClick.AddListener(() => ReplayTransition.LoadNextLevel(SceneManager.GetActiveScene().name)); //Add a listener to alow player to reload the game
        reloadInstructionsButton.onClick.AddListener(() => bossChatTutorialIntro()); //Add a listener to alow player to reload the instructions
        logOutButton.onClick.AddListener(() => stopPlayerExit()); //Add a listener to the log out button to show the boss chat object, easter egg
    }

    //This will show a random message from the boss chat object, when the player tries to log out
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

        //Randomly picks an idex for the array above
        int randomIndex = Random.Range(0, stopLogoutDialogues.Length);




        bossChatPrefab.SetActive(true); //Show the boss chat object
        dialogueText = bossChatPrefab.transform.Find("Panel").Find("BossDialogue").GetComponent<Text>(); //Find the Boss Dialogue Text Component
        continueButton = bossChatPrefab.transform.Find("Panel").Find("Continue").GetComponent<Button>(); //Find the Continue Button Component


        continueButton.onClick.AddListener(OnContinuePressed); //Add a listener to the continue button to proceed to the next message
        dialogueText.text = "";         // Clear the dialogue box initially
        continueButton.gameObject.SetActive(false); // Hide the button at first
        ShowMessage(stopLogoutDialogues[randomIndex]); //Show the random message from the array using random generated index
    }
}


    






