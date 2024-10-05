using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoom : MonoBehaviour
{
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

        messageArray = new string[] {
            "Welcome new recruit.",
            "Our pizza shop is in danger of being hacked.",
            "You will ensure the safety of our network starting with setting up the anti-virus software.",
            "Click the anti-virus folder to begin your first task."
        };

        startChatButton.onClick.AddListener(OnStartChatClicked);
        messageText.gameObject.SetActive(false);
        chatImage.gameObject.SetActive(false);
        bossIcon.gameObject.SetActive(false);
        clickToContinue.gameObject.SetActive(false);
    }

    private void OnStartChatClicked() {
        startChatButton.gameObject.SetActive(false);
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

    private IEnumerator ShowClickToContinueAfterTextIsFinished() {
        while (textWriterSingle != null && textWriterSingle.IsActive()) {
            yield return null;
        }
        yield return new WaitForSeconds(delay);

        if (currentMessageIndex < messageArray.Length) {
        clickToContinue.gameObject.SetActive(true);
    }
    }

    private void HideClickToContinue() {
        clickToContinue.gameObject.SetActive(false);
    }
}
