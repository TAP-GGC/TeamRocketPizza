using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TDBossChat : MonoBehaviour
{
    public Text uiText; // For regular Text component
    public float typingSpeed; // Delay between each character

    private string[] dialogues; // Array to hold multiple dialogues
    private int currentDialogueIndex = 0; // Track the current dialogue
    private bool isTyping;

    // Reference to the BoxCollider2D of the textbox
    private RectTransform textBoxRectTransform;
    private CanvasGroup canvasGroup;

    private Button textButton;
    void Start()
    {
        // Get the BoxCollider2D of the textbox
        canvasGroup = GameObject.Find("BossChat").GetComponent<CanvasGroup>();
        textBoxRectTransform = uiText.GetComponent<RectTransform>();
        textButton = GameObject.Find("NextButton").GetComponent<Button>();
        if (textButton != null)
        {
            // Add a listener to the button to call the ContinueDialogue method when clicked
            textButton.onClick.AddListener(ContinueDialogue);
        }
    }

    public void StartTyping(string[] messages)
    {
        if (isTyping) return; // Prevent multiple calls
        dialogues = messages;
        currentDialogueIndex = 0;
        
        StartCoroutine(TypeNextDialogue());
    }
private enum DialogueState
{
    Typing,
    WaitingForInput,
    Finished
}

private DialogueState currentState = DialogueState.Finished;

private IEnumerator TypeNextDialogue()
{
    while (currentDialogueIndex < dialogues.Length)
    {
        currentState = DialogueState.Typing;
        
        string currentText = dialogues[currentDialogueIndex];
        uiText.text = "";


        for (int i = 0; i < currentText.Length; i++)
        {
            if (currentState != DialogueState.Typing)
                {
                    // If current state changes, complete the dialogue instantly
                    uiText.text = currentText;
                    break;
                }
            uiText.text += currentText[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        currentState = DialogueState.WaitingForInput; 
        
        if (currentText == "")
            {
                currentState = DialogueState.Finished;
                canvasGroup.alpha = 0f; // Hide the canvas
                canvasGroup.interactable = false; 
                canvasGroup.blocksRaycasts = false;
            }
            // Wait until the button is clicked to continue
        yield return new WaitUntil(() => currentState == DialogueState.Finished);
        currentDialogueIndex++;
    }

    currentState = DialogueState.Finished;
    Debug.Log("All dialogues finished.");
}

private void ContinueDialogue()
{
    if (currentState == DialogueState.WaitingForInput)
    {
        currentState = DialogueState.Finished; // Move to finished state
        Debug.Log("Continuing to the next dialogue...");
    }
    if (currentState == DialogueState.Typing)
    {
        uiText.text = dialogues[currentDialogueIndex];
        currentState = DialogueState.WaitingForInput; // Move to finished state
        Debug.Log("Continuing to the next dialogue...");
    }
}
}
