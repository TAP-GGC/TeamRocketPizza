using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using CodeMonkey.utils;

public class ChatRoom : MonoBehaviour
{
    private Text messageText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private int currentMessageIndex = 0;

    // private void Awake() {
    //     messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();

    //     transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
    //         if (textWriterSingle != null && textWriterSingle.IsActive()) {
    //             //Currently active TextWriter
    //             textWriterSingle.WriteAllAndDestroy();
    //         } else {
    //             string[] messageArray = new string[] {
    //             "Welcome new recruit.",
    //             "I am your boss and I will be informing you of your tasks.",
    //             "Our pizza shop is in danger of being hacked.",
    //             "You will ensure the safety of our network starting with setting up the anti-virus.",
    //             "Click the anti-virus folder to begin your first task."
    //             };
    //         }

    //         // string message = messageArray[Random.Range(0, messageArray.Length)];
    //         // textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true);
    //         if (currentMessageIndex < messageArray.Length) {
    //             string message = messageArray[currentMessageIndex];
    //             currentMessageIndex++; // Move to the next message for the next click
    //             textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true);
    //         }
    //     };
    // }

    private void Awake() {
    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();

    transform.Find("message").GetComponent<Button_UI>().ClickFunc = () => {
        if (textWriterSingle != null && textWriterSingle.IsActive()) {
            // Currently active TextWriter
            textWriterSingle.WriteAllAndDestroy();
        } else {
            string[] messageArray = new string[] {
                "Welcome new recruit.",
                "I am your boss and I will be informing you of your tasks.",
                "Our pizza shop is in danger of being hacked.",
                "You will ensure the safety of our network starting with setting up the anti-virus.",
                "Click the anti-virus folder to begin your first task."
            };

            // Ensure we don't go out of bounds in the array
            if (currentMessageIndex < messageArray.Length) {
                string message = messageArray[currentMessageIndex];
                currentMessageIndex++; // Move to the next message for the next click
                textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
            }
        }
    };
}

    private void Start() {
        //TextWriter.AddWriter_Static(messageText, "codeBlock", 0.01f, true, true);
    }
}
