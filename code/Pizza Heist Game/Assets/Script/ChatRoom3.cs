using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoom3 : MonoBehaviour
{
    private Text messageText;
    private Text clickToContinue;
    private TextWriter.TextWriterSingle textWriterSingle;
    private string[] messageArray;
    private int currentMessageIndex = 0;
    public float delay = 5f;

    private void Awake() {
    messageText = transform.Find("message").Find("bossMessage").GetComponent<Text>();
    clickToContinue = transform.Find("message2").Find("clickToContinue").GetComponent<Text>();
    clickToContinue.gameObject.SetActive(false);

    messageArray = new string[] {
        "Good work, new recruit.",
        "You've completed all of your tasks.",
        "It's about time you clock out for the day."
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
