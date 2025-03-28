//This file just prints decorative text on the Desktop UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainJava : MonoBehaviour
{
    private Text messageText;

    private void Awake() {
        messageText = GameObject.Find("messageText").GetComponent<Text>();
    }

    private void Start() {
        StartCoroutine(DelayTextDisplay());
    }

    private IEnumerator DelayTextDisplay() {
        yield return new WaitForSeconds(1f);

        //Message to be displayed
        string codeBlock = 
        "public class AuthorizedAccess {\n" +
        "\n" +
        "    public static void authorizedAccess() throws InterruptedException {\n" +
        "        System.out.println(\"Initiating secure connection to target system for testing...\");\n" +
        "        Thread.sleep(2000);\n" +
        "\n" +
        "        for (int i = 0; i <= 100; i++) {\n" +
        "            Thread.sleep(30);\n" +
        "            System.out.print(\"Establishing secure connection... \" + i + \"% complete\\r\");\n" +
        "        }\n" +
        "\n" +
        "        System.out.println(\"\\nConnection established successfully!\");\n" +
        "        Thread.sleep(1000);\n" +
        "\n" +
        "        System.out.println(\"Running security checks with authorized credentials...\");\n" +
        "        Thread.sleep(1000);\n" +
        "\n" +
        "        System.out.println(\"Root access obtained for testing purposes only. Proceeding with vulnerability assessment.\");\n" +
        "    }\n" +
        "\n" +
        "    public static void main(String[] args) throws InterruptedException {\n" +
        "        authorizedAccess();\n" +
        "    }\n" +
        "}";
        TextWriter.AddWriter_Static(messageText, codeBlock, 0.01f, true, false); //Prints out the text
    }
}
