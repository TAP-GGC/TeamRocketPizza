using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class InputHandlerCopy : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] InputField userName;
    [SerializeField] Text resultText;
    [SerializeField] Button submitButton;
    public LevelLoader transitionLoad;
    Boolean login;
    public Image accessBar;
    public Text accessGranted;

    private void Awake() {
        accessBar.gameObject.SetActive(false);
        accessGranted.gameObject.SetActive(false);
    }

        private void Start() 
    {
       
        submitButton.onClick.AddListener(ValidateInput);
    }

    public void ValidateInput() {
        string input = inputField.text;
        login = false;

        // Check if the password length is between 6 and 12 characters
        if (input.Length < 12 || input.Length > 24)
        {
            resultText.text = "Password must be between 12 and 24 characters.";
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

        login = true;

        if (login) 
        {
            inputField.gameObject.SetActive(false);
            userName.gameObject.SetActive(false);
            accessBar.gameObject.SetActive(true);
            StartCoroutine(HandleAccessGranted());
        }
    }

        private IEnumerator HandleAccessGranted() {
        yield return new WaitForSeconds(0.5f);
        string correctMessage = "Access Granted";
        string jumbledMessage = ShuffleString(correctMessage);
        accessGranted.text = jumbledMessage; 
        accessGranted.gameObject.SetActive(true); 

        for (int i = 0; i < correctMessage.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            char[] messageArray = jumbledMessage.ToCharArray();
            messageArray[i] = correctMessage[i]; 
            jumbledMessage = new string(messageArray);
            accessGranted.text = jumbledMessage; 
        }

        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in.";
        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in..";
        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in...";
        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in.";
        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in..";
        yield return new WaitForSeconds(0.2f);
        resultText.text = "Logging in...";

        yield return new WaitForSeconds(1f);
        NavigateToDesktop();
    }

    private string ShuffleString(string input)
    {
        char[] characters = input.ToCharArray();
        System.Random rand = new System.Random();
        for (int i = 0; i < characters.Length; i++)
        {
            int randomIndex = rand.Next(0, characters.Length);
            char temp = characters[i];
            characters[i] = characters[randomIndex];
            characters[randomIndex] = temp;
        }
        return new string(characters);
    }

    public void NavigateToDesktop() {
        transitionLoad.LoadNextLevel("Desktop");
    }
}
