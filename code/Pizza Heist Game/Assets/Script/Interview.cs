using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interview : MonoBehaviour
{
    //public Button NextCutsceneButton;
    private Text messageText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private string[] messageArray;
    private int currentMessageIndex = 0;
    public Animator anim;

    private void Awake() {
    messageText = transform.Find("message").Find("messageText").GetComponent<Text>();
    messageArray = new string[] {
        "Welcome.",
        "I have a job for you.",
        "But before you begin, you must create an account in our database.",
        "Decide on a username and develop a secure password.",
        "You won't be granted access until your password is secure enough.",
        "I expect great work from you today.",
        "...",
        "I hope this will be enough compensation."
    };

    //NextCutsceneButton.gameObject.SetActive(false);

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
        // } else {
        //     NextCutsceneButton.gameObject.SetActive(true);
        // }
    }

    private void Update(){
        StartCoroutine(waitAnimation());
    }

    private IEnumerator waitAnimation(){
        if(currentMessageIndex == messageArray.Length-1){
            yield return new WaitForSeconds(2f);
            anim.SetTrigger("Slide");
            yield return new WaitForSeconds(0.2f);
            anim.SetTrigger("Floating");
        }
    }

}
