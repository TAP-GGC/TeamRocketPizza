using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Text resultText;

    public void ValidateInput() {
        string input = inputField.text;

        if (input.Length < 4) {
            resultText.text = "Invalid Input";
            resultText.color = Color.red;
        } else {
            resultText.text = "Valid Input";
            resultText.color = Color.green;
        }
    }
}
