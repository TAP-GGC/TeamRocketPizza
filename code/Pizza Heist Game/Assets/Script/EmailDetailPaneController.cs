using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailDetailPaneController : MonoBehaviour
{
    // Start is called before the first frame update
    private Email email;
    Button notPhishingButton;
    Button phishingButton;


    void Start()
    {
        notPhishingButton = GameObject.FindWithTag("NotPhishing").GetComponent<Button>();
        phishingButton = GameObject.FindWithTag("IsPhishing").GetComponent<Button>();


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
        notPhishingButton.onClick.AddListener(() => CheckPhishing(notPhishingButton));
        phishingButton.onClick.AddListener(() => CheckPhishing(phishingButton));
    }


    public void CheckPhishing(Button button)
    {
        // Check if the player's answer is correct
        //If correct Destory the email list item object and email details object
        //If incorrect show feedback, then destroy the email list item object and email details object

        
        if (email.IsPhishing && button.tag == "IsPhishing")
        {
            Debug.Log("Correct");
        }
        else if (!email.IsPhishing && button.tag == "NotPhishing")
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log(email.phishingExplanation);
        }
    }
    



}