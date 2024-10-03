using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interview : MonoBehaviour
{
    private Text messageText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private string[] messageArray;
    private int currentMessageIndex = 0;

    private void Awake() {
    messageText = transform.Find("message").Find("messageText").GetComponent<Text>();
    messageArray = new string[] {
        "Welcome.",
        "I have been waiting for your arrival.",
        "Big Caesars is in need of your help. Our pizza rivals, Mama Johns, are trying to get their hands on our secret pizza formula."
    };
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

    private void ShowNextMessage() {
        if (currentMessageIndex < messageArray.Length) {
            string message = messageArray[currentMessageIndex];
            currentMessageIndex++;
            textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
        }
    }

}
