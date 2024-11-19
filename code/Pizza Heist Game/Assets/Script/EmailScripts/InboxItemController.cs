using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Title: InboxItemController
 * Description: Holds/Controls the InboxItem GameObjects in the Inbox scene.
 * Author: Brian Ramos Cazares
 */




public class InboxItemController : MonoBehaviour
{

    public Email email; // The email that this InboxItem represents

    public void SetEmail(Email email) // Set the email that this InboxItem represents
    {
        this.email = email; // Set the email
    }
    public Email GetEmail() // Get the email that this InboxItem represents
    {
        return email; // Return the email
    }

}
