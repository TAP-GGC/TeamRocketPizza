using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TDBossChat typewriterEffect; // Reference to the TypewriterEffect component

    private void Start()
    {
        // Example array of dialogues
        string[] dialogues = new string[]
        {
            "Welcome To Virus Tower Defense. In this task you will help defend our system from malicious cyber attack called viruses.",
            "These viruses are trying to get to the exit. TO stop them, go ahead and drag a tower from the shop menu onto any square slot.",
            "You can always sell you tower by right clicking the tower. Be careful, when you sell it will only refund you 1/4 of its cost.",
            "Each tower has a purpose and are important to understand in order to progress.",
            "To start off just place down a normal Malware Tower. go ahead and drag a tower from the shop menu onto any square slot.",
            "",
        };

        // Start typing the dialogues
        typewriterEffect.StartTyping(dialogues);
    }
}
