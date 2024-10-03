using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Text resultText;
    [SerializeField] Button submitButton;
    private NavController nav;
    Boolean login;

    private void Start() 
    {
        nav = FindObjectOfType<NavController>();
    }

    public void ValidateInput() {
        string input = inputField.text;
        login = false;

        // Check if the password length is between 6 and 12 characters
        if (input.Length < 6 || input.Length > 12)
        {
            resultText.text = "Password must be between 6 and 12 characters.";
            return;
        }

        // Check if password contains at least one uppercase letter
        if (!Regex.IsMatch(input, "[A-Z]"))
        {
            resultText.text = "Password must contain at least one uppercase letter.";
            return;
        }

        // Check if password contains at least one lowercase letter
        if (!Regex.IsMatch(input, "[a-z]"))
        {
            resultText.text = "Password must contain at least one lowercase letter.";
            return;
        }

        // Check if password contains at least one number
        if (!Regex.IsMatch(input, "[0-9]"))
        {
            resultText.text = "Password must contain at least one number.";
            return;
        }

        // Check if password contains at least one special character
        if (!Regex.IsMatch(input, "[^a-zA-Z0-9]"))
        {
            resultText.text = "Password must contain at least one special character.";
            return;
        }

        // If all checks pass
        //resultText.text = "Account Created.";
        login = true;

        if (login) 
        {
            nav.GoToScene("Desktop");
        }
    }
}
