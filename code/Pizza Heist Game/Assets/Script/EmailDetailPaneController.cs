using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmailDetailPaneController : MonoBehaviour
{
    // Start is called before the first frame update
    private Email email;
    Button notPhishingButton;
    Button phishingButton;

    private Email currentEmail;  // The current email object
    private GameObject inboxEntry; // The related inbox entry GameObject

    // Delegate and event to notify the PhishingGameManager
    public delegate void EmailAction(Email email, GameObject inboxEntry, bool isCorrectGuess);
    public event EmailAction OnEmailProcessed;


    public void SetUp(Email email, GameObject associatedInboxEntry)
    {
        currentEmail = email;
        inboxEntry = associatedInboxEntry;

        AddListenersToButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmail(Email email)
    {
        this.email = email;
    }

    public void AddListenersToButtons()
    {
        // Set up the button listeners
        phishingButton.onClick.AddListener(OnPhishingButtonClicked);
        notPhishingButton.onClick.AddListener(OnNotPhishingButtonClicked);
    }


    // public void CheckPhishing(Button button)
    // {
    //     // Check if the player's answer is correct
    //     //If correct Destory the email list item object and email details object
    //     //If incorrect show feedback, then destroy the email list item object and email details object

    //     bool isCorrect = currentEmail.IsPhishing;
    //     if (email.IsPhishing && button.tag == "IsPhishing")
    //     {
    //         Debug.Log("Correct");
    //     }
    //     else if (!email.IsPhishing && button.tag == "NotPhishing")
    //     {
    //         Debug.Log("Correct");
    //     }
    //     else
    //     {
    //         Debug.Log(email.phishingExplanation);
    //     }

    //     // Notify the PhishingGameManager with the result
    //     OnEmailProcessed?.Invoke(currentEmail, inboxEntry, isCorrect);
    // }

    private void OnPhishingButtonClicked()
    {
        bool isCorrect = currentEmail.IsPhishing;
        if (isCorrect)
        {
            Debug.Log("Correct! This was a phishing email.");
        }
        else
        {
            Debug.Log("Incorrect. This was not a phishing email.");
        }

        // Notify the PhishingGameManager with the result
        OnEmailProcessed?.Invoke(currentEmail, inboxEntry, isCorrect);
    }

    private void OnNotPhishingButtonClicked()
    {
        bool isCorrect = !currentEmail.IsPhishing;
        if (isCorrect)
        {
            Debug.Log("Correct! This was not a phishing email.");
        }
        else
        {
            Debug.Log("Incorrect. This was a phishing email.");
        }

        // Notify the PhishingGameManager with the result
        OnEmailProcessed?.Invoke(currentEmail, inboxEntry, isCorrect);
    }
    



}
