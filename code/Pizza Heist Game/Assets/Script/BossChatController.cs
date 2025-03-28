using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossChatController : MonoBehaviour
{
    public Button startChatButton;
    public Image chatImage;
    public Image bossIcon;
    private Text messageText;
    private Text clickToContinue;
    private TextWriter.TextWriterSingle textWriterSingle;
    private int currentMessageIndex = 0;
    public float delay = 60f;
    public string[] messageArray;

    private void Awake() {
    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();
    clickToContinue = transform.Find("message2").Find("clickToContinue").GetComponent<Text>();

        //Array of messages to be displayed
        messageArray = new string[] {
            "Welcome new recruit.",
            "Congratulations on logging onto your desktop for the first time.",
            "Hopefully you made a secure password.",
            "I will only communicate with you through chat messages.",
            "Do not expect any emails from me.",
            "Our pizza shop is in danger of being hacked.",
            "You will ensure the safety of our network starting with setting up the anti-virus.",
            "Click the anti-virus folder to begin your first task."
        };

        messageText.gameObject.SetActive(false);
        chatImage.gameObject.SetActive(false);
        bossIcon.gameObject.SetActive(false);
        clickToContinue.gameObject.SetActive(false);
    }

    public void setMessageArray(string[] messages) {
        messageArray = messages;
    }

    public void OnStartChatClicked() {
        chatImage.gameObject.SetActive(true);
        StartCoroutine(DelayedChatStart());
    }

    private IEnumerator DelayedChatStart() {
        yield return new WaitForSeconds(1f);

        messageText.gameObject.SetActive(true);
        bossIcon.gameObject.SetActive(true);

        ShowNextMessage();

        if (currentMessageIndex < messageArray.Length) {
            string initialMessage = messageArray[currentMessageIndex];
            currentMessageIndex++;
            textWriterSingle = TextWriter.AddWriter_Static(messageText, initialMessage, .05f, true, true);
            StartCoroutine(ShowClickToContinueAfterTextIsFinished());
        }

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
        // Wait until the text finishes writing
        while (textWriterSingle != null && textWriterSingle.IsActive()) {
            yield return null;
        }

        // Now wait for the specified delay
        yield return new WaitForSeconds(delay);

        Debug.Log("Showing 'click to continue' text.");
        clickToContinue.gameObject.SetActive(true); // Show the "click to continue" text
    }

    private void HideClickToContinue() {
        clickToContinue.gameObject.SetActive(false); // Hide the "click to continue" text when clicked
    }
}
