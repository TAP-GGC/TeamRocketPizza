using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
 * Title: EmailDetailPaneController
 * Description: Holds/Controls the EmailDetailPane GameObjects in the EmailDetailPane scene.
 * Author: Brian Ramos Cazares
 * 
 * Note:
 *    - This Scripts interacts with the Phishing/NotPhishing buttons in the EmailDetailPane scene.
 */

public class EmailDetailPaneController : MonoBehaviour
{
    private Email email; // The email that this InboxItem represents
    public Button notPhishingButton; // The Not Phishing button
    public Button phishingButton; // The Phishing button

    public Email currentEmail;  // The current email object
    public GameObject inboxEntry; // The related inbox entry GameObject

    // Delegate and event to notify the PhishingGameManager
    public delegate void EmailAction(Email email, GameObject inboxEntry, bool isCorrectGuess);


    public void SetUp(Email email, GameObject associatedInboxEntry) // Set the email that this InboxItem will represent
    {
        currentEmail = email; // Set the email
        inboxEntry = associatedInboxEntry; // Set the associated inbox entry

        notPhishingButton = GameObject.FindWithTag("NotPhishing").GetComponent<Button>(); // Find the Not Phishing button
        phishingButton = GameObject.FindWithTag("IsPhishing").GetComponent<Button>(); // Find the Phishing button

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmail(Email email) // Set the email that this InboxItem represents
    {
        this.email = email; // Set the email
    }

}
