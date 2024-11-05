using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InboxItemController : MonoBehaviour
{
    // Start is called before the first frame update
    public Email email;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmail(Email email)
    {
        this.email = email;
    }
    public Email GetEmail()
    {
        return email;
    }

}
