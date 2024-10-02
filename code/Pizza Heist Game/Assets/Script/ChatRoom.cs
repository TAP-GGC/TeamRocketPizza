using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoom : MonoBehaviour
{
    public Button startChatButton;
    public Image chatImage;
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

        startChatButton.onClick.AddListener(OnStartChatClicked);
        messageText.gameObject.SetActive(false);
        chatImage.gameObject.SetActive(false);
    }

        private void OnStartChatClicked() {

        startChatButton.gameObject.SetActive(false);

        messageText.gameObject.SetActive(true);
        chatImage.gameObject.SetActive(true);

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
}
}
