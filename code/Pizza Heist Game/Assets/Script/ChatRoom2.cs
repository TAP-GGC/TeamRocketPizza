using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using CodeMonkey.utils;

public class ChatRoom2 : MonoBehaviour
{
    private Text messageText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private int currentMessageIndex = 0;
    private string[] messageArray;

    private void Awake() {
    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();

        messageArray = new string[] {
            "Welcome new recruit.",
            "I am your boss and I will be informing you of your tasks.",
            "Our pizza shop is in danger of being hacked.",
            "You will ensure the safety of our network starting with setting up the anti-virus.",
            "Click the anti-virus folder to begin your first task."
        };
    }

        private void OnStartChatClicked() {

        messageText.gameObject.SetActive(true);

        if (currentMessageIndex < messageArray.Length) {
            string initialMessage = messageArray[currentMessageIndex];
            currentMessageIndex++;
            textWriterSingle = TextWriter.AddWriter_Static(messageText, initialMessage, .05f, true, true);
        }

        transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
            if (textWriterSingle != null && textWriterSingle.IsActive()) {
                textWriterSingle.WriteAllAndDestroy();
            } else {
                if (currentMessageIndex < messageArray.Length) {
                    string message = messageArray[currentMessageIndex];
                    currentMessageIndex++;
                    textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
                }
            }
        };

    // transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
    //     if (textWriterSingle != null && textWriterSingle.IsActive()) {
    //         // Currently active TextWriter
    //         textWriterSingle.WriteAllAndDestroy();
    //     } else {
    //         string[] messageArray = new string[] {
    //             "I am your boss and I will be informing you of your tasks.",
    //             "Our pizza shop is in danger of being hacked.",
    //             "You will ensure the safety of our network starting with setting up the anti-virus.",
    //             "Click the anti-virus folder to begin your first task."
    //         };

    //         // Ensure we don't go out of bounds in the array
    //         if (currentMessageIndex < messageArray.Length) {
    //             string message = messageArray[currentMessageIndex];
    //             currentMessageIndex++; // Move to the next message for the next click
    //             textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
    //         }
    //     }
    // };
}

    private void Start() {
        if (currentMessageIndex < messageArray.Length) {
            string initialMessage = messageArray[currentMessageIndex];
            currentMessageIndex++;
            textWriterSingle = TextWriter.AddWriter_Static(messageText, initialMessage, .05f, true, true);
        }
    }
}
