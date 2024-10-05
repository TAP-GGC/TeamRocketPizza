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

    messageArray = new string[] {
        "Great job. The Anti-virus software is now up and running.",
        "We've recieved an influx of emails.",
        "We need you to identify and report all of the phishing emails.",
        "Click on the E-mail folder to get started."
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
        HideClickToContinue();
        string message = messageArray[currentMessageIndex];
        currentMessageIndex++;
        textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .05f, true, true);
        StartCoroutine(ShowClickToContinueAfterTextIsFinished());
        }
    }

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

    private void HideClickToContinue() {
        clickToContinue.gameObject.SetActive(false);
    }

}
