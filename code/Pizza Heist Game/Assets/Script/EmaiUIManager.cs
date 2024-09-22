using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailUIManager : MonoBehaviour
{
    public GameObject emailListItemPrefab;  // Prefab for list item (button or text)
    public Transform emailListContent;      // Content area of the scroll view
    public EmailDetailsUI emailDetailsUI;   // Reference to the UI for displaying email details

    private List<Email> emailList = new List<Email>();

    void Start()
    {
        LoadEmails();
        PopulateEmailList();
    }

    void LoadEmails()
    {
        // You would load emails from JSON here (omitted for brevity)
        // For example:
        // emailList = ... (load from your JSON file)
    }

    void PopulateEmailList()
    {
        foreach (Email email in emailList)
        {
            GameObject emailListItem = Instantiate(emailListItemPrefab, emailListContent);
            EmailListItemUI itemUI = emailListItem.GetComponent<EmailListItemUI>();
            itemUI.Setup(email, emailDetailsUI);
        }
    }
}
