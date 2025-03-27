using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoom2 : MonoBehaviour
{
    private Text messageText;
    private Text clickToContinue;
    private TextWriter.TextWriterSingle textWriterSingle;
    private string[] messageArray;
    private int currentMessageIndex = 0;
    public float delay = 10f;

    private void Awake() {
    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();
    clickToContinue = transform.Find("message2").Find("clickToContinue").GetComponent<Text>();
    clickToContinue.gameObject.SetActive(false);

    //Array of messages to be displayed
    messageArray = new string[] {
        "Oh shoot! Rookie, I have bad news.",
        "I clicked on a phishing link and now the shop is compromised.",
        "ksj7^%Bu we%$NSpw9 2&456",
        "Quick! There's still a way to fix it!",
        "We have an advanced anti-virus program built just for this.",
        "IOkl j*&687sjK J^%456",
        "Once you get it working, it will be as if none of this happened in the first place!",
        "It will detect irregular patterns and defend against anything malicious.",
        "us*89sdJH &*65hdHJ KJ^&*989fsd",
        "It's a bit complicated, but I'll walk you through it.",
        "Click on the 'Anti-Virus' folder. Hurry!"
    };

        //Functionality to continue the chat when the user clicks on the message box
        transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
            if (textWriterSingle != null && textWriterSingle.IsActive()) {
                textWriterSingle.WriteAllAndDestroy();
            } else {
                ShowNextMessage();
            }
        };
    }

        private void Start() {
            ShowNextMessage();
    }

    //Prints each message in the array
    private void ShowNextMessage() {
        if (currentMessageIndex < messageArray.Length) {
        HideClickToContinue();
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
        } else {
            clickToContinue.gameObject.SetActive(false);
        }
    }

    //Hides "Click To Continue" message
    private void HideClickToContinue() {
        clickToContinue.gameObject.SetActive(false);
    }

}
