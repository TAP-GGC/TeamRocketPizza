using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainJava : MonoBehaviour
{

    [SerializeField] private TextWriter textWriter;
    private Text messageText;

    private void Awake() {
        messageText = transform.Find("message").Find("messageText").GetComponent<Text>();
    }

    private void Start() {
        textWriter.addWriter(messageText, "Hello World!", 0.5f);
    }
}
