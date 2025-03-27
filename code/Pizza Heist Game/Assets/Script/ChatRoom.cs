using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoom : MonoBehaviour
{
    //References to UI components
    public Button startChatButton;
    public Image chatImage;
    public Image bossIcon;
    private Text messageText;
    private Text clickToContinue;

    private TextWriter.TextWriterSingle textWriterSingle;
    private int currentMessageIndex = 0;
    public float delay = 30f;
    private string[] messageArray;

    private void Awake() {

    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();
    clickToContinue = transform.Find("message2").Find("clickToContinue").GetComponent<Text>();
    
        //Array of messages to be displayed
        messageArray = new string[] {
            "Welcome new recruit.",
            "Welcome new recruit.",
            "Congratulations on successfully logging in.",
            "I'll walk you through everything since it's your first day.",
            "The folders on your screen will help you complete today's tasks.",
            "We're going to start off simple.",
            "We recieved an influx of emails so we need your help to sort it all",
            "Click on the 'E-mail' folder when you're ready to begin."
            
        };
        
        startChatButton.onClick.AddListener(OnStartChatClicked); //Event listener to start chat when clicked

        //Initially hides UI components
        messageText.gameObject.SetActive(false);
        chatImage.gameObject.SetActive(false);
        bossIcon.gameObject.SetActive(false);
        clickToContinue.gameObject.SetActive(false);
    }

    private void OnStartChatClicked() {
        startChatButton.gameObject.SetActive(false); //Hides start button when the chat starts
        
        chatImage.gameObject.SetActive(true); //Shows chat UI and begins coroutine
        StartCoroutine(DelayedChatStart());
    }

    private IEnumerator DelayedChatStart() {
        yield return new WaitForSeconds(1f);

        //Shows message text box and boss icon
        messageText.gameObject.SetActive(true);
        bossIcon.gameObject.SetActive(true);

        ShowNextMessage();

        //Checks if there are more messages to print and repeats the process until there are no more
        if (currentMessageIndex < messageArray.Length) {
            string initialMessage = messageArray[currentMessageIndex];
            currentMessageIndex++;
            textWriterSingle = TextWriter.AddWriter_Static(messageText, initialMessage, .05f, true, true); //Text writer
            StartCoroutine(ShowClickToContinueAfterTextIsFinished()); 
        }

        //Functionality to continue the chat when the user clicks on the message box
        transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
            if (textWriterSingle != null && textWriterSingle.IsActive()) {
                textWriterSingle.WriteAllAndDestroy();
            } else {
                if (currentMessageIndex < messageArray.Length) {
                    HideClickToContinue();
                    string message = messageArray[currentMessageIndex];
                    currentMessageIndex++;
                    textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
                    StartCoroutine(ShowClickToContinueAfterTextIsFinished());
                }
            }
        };
    }

    //Prints each message in the array
    private void ShowNextMessage() {
    if (currentMessageIndex < messageArray.Length) {
        string message = messageArray[currentMessageIndex];
        currentMessageIndex++;
        textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
        StartCoroutine(ShowClickToContinueAfterTextIsFinished());
    }
}

    //Shows "Click To Continue" message after a set amount of time after the current line of text is finished printing
    private IEnumerator ShowClickToContinueAfterTextIsFinished() {
        while (textWriterSingle != null && textWriterSingle.IsActive()) {
            yield return null;
        }
        yield return new WaitForSeconds(delay);

        if (currentMessageIndex < messageArray.Length) {
        clickToContinue.gameObject.SetActive(true);
    }
    }

    //Hides "Click To Continue" message
    private void HideClickToContinue() {
        clickToContinue.gameObject.SetActive(false);
    }
}
