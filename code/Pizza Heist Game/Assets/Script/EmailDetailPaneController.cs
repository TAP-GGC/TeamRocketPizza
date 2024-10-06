using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmailDetailPaneController : MonoBehaviour
{
    // Start is called before the first frame update
    private Email email;
    public Button notPhishingButton;
    public Button phishingButton;

    public Email currentEmail;  // The current email object
    public GameObject inboxEntry; // The related inbox entry GameObject

    // Delegate and event to notify the PhishingGameManager
    public delegate void EmailAction(Email email, GameObject inboxEntry, bool isCorrectGuess);
    public event EmailAction OnEmailProcessed;

    


    public void SetUp(Email email, GameObject associatedInboxEntry)
    {
        currentEmail = email;
        inboxEntry = associatedInboxEntry;

        notPhishingButton = GameObject.FindWithTag("NotPhishing").GetComponent<Button>();
        phishingButton = GameObject.FindWithTag("IsPhishing").GetComponent<Button>();

        //AddListenersToButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmail(Email email)
    {
        this.email = email;
    }

    // public void AddListenersToButtons()
    // {
    //     // Set up the button listeners
    //     phishingButton.onClick.AddListener(OnPhishingButtonClicked);
    //     notPhishingButton.onClick.AddListener(OnNotPhishingButtonClicked);
    //     Debug.Log("Listeners added");
    // }


    // private void OnPhishingButtonClicked() // UNUSED
    // {
    //     Debug.Log("Phishing button clicked");
    //     bool isCorrect = currentEmail.IsPhishing;
    //     if (isCorrect)
    //     {
    //         Debug.Log("Correct! This was a phishing email.");
    //     }
    //     else
    //     {
    //         Debug.Log("Incorrect. This was not a phishing email.");
    //     }


    //     // Destroy the email list item object and email details object
    //     //Destroy(inboxEntry);
    //     //Destroy(gameObject);

    //     // Notify the PhishingGameManager with the result
    //     OnEmailProcessed?.Invoke(currentEmail, inboxEntry, isCorrect);
    // }

    // private void OnNotPhishingButtonClicked()
    // {
    //     Debug.Log("Not-phishing button clicked");
    //     bool isCorrect = !currentEmail.IsPhishing;
    //     if (isCorrect)
    //     {
    //         Debug.Log("Correct! This was not a phishing email.");
    //     }
    //     else
    //     {
    //         Debug.Log("Incorrect. This was a phishing email.");
    //     }

    //     // Destroy(inboxEntry);
    //     // Destroy(gameObject);

    //     // // Notify the PhishingGameManager with the result
    //     OnEmailProcessed?.Invoke(currentEmail, inboxEntry, isCorrect);
    // }

}
