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
            "These viruses will attack from the left and exit the top of the screen. To stop them utilize the towers as your defenses.",
            "Each tower has a purpose and are important to understand in order to progress.",
            "To start off click on the tower icons to see what they do below. Get familiar with their names!",
            "You can also drag a tower from the shop menu onto any square slot. For now just place the a normal Malware Tower.",
            "You can always sell your tower by right clicking the tower. Be careful, when you sell it, it will only refund you 75% of its cost.",
            "Once you are ready, click the PLAY button above.",
            "",
            "Incoming! A new virus is approaching!",
            "This new virus is called a Ransomware Virus.",
            "Their purpose is to invade your computer in order to take money from you. These guys are ruthless because the only way to get rid of them is to pay the price.",
            "To avoid these be sure to Back-up your files, this way you can restore the previous state before its too late. Leading to getting rid of the virus.",
            "You can utilize the Backup tower.",
            "",
            "Incoming! A new virus is approaching!",
            "Here comes the DoS Attack.",
            "These virus are also know to be called Denial-Of-Service. They can cause all of your Computers to perform really slow.",
            "Be careful! Any towers within it's range will be stun and can't defend for a short period of time.",
            "To destroy these, utilize the Network Defender as they can help prevent the attack.",
            "You can also use the Firewall in order to slow these viruses allowing your defenses to attack easily.",
            "",
            "Incoming! A new virus is approaching!",
            "This virus is called the Worm.",
            "They are also called Computer Worms. They are design to attack the network by self-replicating themselves.",
            "These multiplying enemies can be a problem because they will try to overhelm the anti-virus defenses.",
            "They also can ignore normal Malware towers except for the Network Defender, Firewall, and IDS Tower.",
            "I would recommend using the IDS (Intrusion Detective System), These tower can detect almost anything. They are a very powerful Tool.",
            "",
            "The final virus is approaching!",
            "These viruses are called Tojans.",
            "They can be very deceptive. Trojans will try to hide themselves as a non-harmful entities.",
            "However, once they are detected they will enter in a berserk state, allowing them to gain a boost of Speed and Health.",
            "To defend against them make sure you are well-prepared with many utilities and variety of towers. High Recommendation to use the IDS towers and Firewall.",
            ""
        };

        // Start typing the dialogues
        typewriterEffect.StartTyping(dialogues);
    }
}
