using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Image = UnityEngine.UI.Image;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] waypoints;
    private Text healthText;
    private Text coinText;
    private Text GameState;

    private CanvasGroup _cgroup;
    private CanvasGroup Chatcgroup;
    private CanvasGroup imgGroup;
    public int coins;
    public int health;
    private bool gameOver;
    private GameStateEnum currentState;
    private EnemySpawner enemySpawner;

    private enum GameStateEnum
    {
        StartGame,
        Malware,
        Worm,
        Dos,
        Ransomware,
        Trojan,
        EndGame
    }
     
    private Dictionary<string, string> virusDescriptions;

    private void Awake()
    {
        main = this;
        
    }

    private void Start(){

        currentState = GameStateEnum.Malware;
        Time.timeScale = 0;
        coins = 100;
        health = 20;
        gameOver = false;
        
        virusDescriptions = new Dictionary<string, string>
        {
            { "Malware", "Malware Viruses are normal viruses that run through the system. These viruses represent common malicious software that can disrupt normal operations, steal information, and cause havoc in the digital realm.\n\nEffective defenses: Any Anti-Virus Defenses. \nAbilities: None." },
            { "Worm", "Worm viruses are malware that self-replicate and spread themselves across your network, allowing them to ignore hardware defenders. They mainly attack network systems and can infect it quickly. Quickly defend against them by using a Network Defender.\n\nEffective defenses: Network Tower. \nAbilities: Duplication." },
            { "Trojan", "Trojan viruses disguise themselves as to appear as friendly and nice. Once installed or detected, they will show their malicious intention by opening backdoors for other malware to enter the system. IDS helps with detection against these \n\nEffective defenses: IDS Tower, detect them. Any tower can attack after. \nAbilities: Deception." },
            { "Ransomware", "Ransomware encrypts your files and demands payment for the decryption key. It can cause significant data loss and financial strain you if not addressed promptly. Preventing it begins with frequently backing up your data, and restoring it before the attack.\n\nEffective defenses: BackUp Tower. \nAbilities: Data Encryption." },
            { "DoS", "Denial-of-Service (DoS) attacks overwhelm systems, causing them to crash or become unavailable. They target network resources and can disrupt service for users. IDS (Intrusion Detection System) helps detects these threats and prevent them from pursuing.\n\nEffective defenses: IDS Tower. \nAbilities: Traffic Monitoring." }
        };

        enemySpawner = GetComponent<EnemySpawner>();
        _cgroup = GameObject.Find("StateMenu").GetComponent<CanvasGroup>();
        Chatcgroup = GameObject.Find("BossChat").GetComponent<CanvasGroup>();
        imgGroup = GameObject.Find("VirusPanel").GetComponent<CanvasGroup>();
        GameState = GameObject.Find("GameState").GetComponent<Text>();
        
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        coinText = GameObject.Find("CoinsText").GetComponent<Text>();
    }

    public void IncreaseCoin(int amount){
        coins += amount;
    }

    public bool SpendCoins(int amount){
        if(amount <= coins){
            coins -= amount;
            return true;
        }
        else{
            Debug.Log("You dont have enough money.");
            return false;
        }
    }

    public void sellTurret(int amount){
        coins += amount;
    }

    public void decreaseHealth(int amount){
        health -= amount;
        if(health <= 0){
            gameOver = true;
            
        }
    }

    public void TextSwitch()
    {
        switch (currentState)
        {
            case GameStateEnum.StartGame:
                SceneManager.LoadScene("TDGame");
                break;
            case GameStateEnum.Malware:
                HideAllGroup();
                ShowGroup();
                currentState = GameStateEnum.Worm; // Transition to hide Malware
                break;
            case GameStateEnum.Worm:
                HideAllGroup();
                ShowGroup();
                currentState = GameStateEnum.Dos; // Transition to Worm detail
                break;
            case GameStateEnum.Dos:
                HideAllGroup();
                ShowGroup();
                currentState = GameStateEnum.Ransomware; // Transition to hide Worm
                break;
            case GameStateEnum.Ransomware:
                HideAllGroup();
                ShowGroup();
                currentState = GameStateEnum.Trojan; // Transition to start the game
                break;
            case GameStateEnum.Trojan:
                HideAllGroup();
                ShowGroup();
                currentState = GameStateEnum.EndGame; // Transition to hide Worm
                break;
            case GameStateEnum.EndGame:
                Time.timeScale = 1;
                SceneManager.LoadScene("Desktop 2");
                break;
        }
    }

    public void WinGame(){

        GameState.text = "You Win! \nClick button to go back";
        _cgroup.alpha = 1f;
        GameState.fontSize = 100;
        _cgroup.interactable = true;
        Time.timeScale = 0;
        currentState = GameStateEnum.EndGame;
    }

    public void LoseGame(){
        
        GameState.text = "Game Over!\nYou Lose\nClick to Restart";
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        Time.timeScale = 0;

        currentState = GameStateEnum.StartGame;
    }
    public void HideAllGroup(){
        GameState.text = "";
        imgGroup.alpha = 0f;
        _cgroup.alpha = 0f;
        _cgroup.interactable = false;
        
    }

    public void ShowGroup(){
        imgGroup.alpha = 1f;
        Chatcgroup.alpha = 1f;
        Chatcgroup.interactable = true;
        Time.timeScale = 1;
    }

    public void Warning()
    {
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        Time.timeScale = 0;
        GameState.fontSize = 120;
        GameState.text = "Warning! A New Virus is Approaching";
        
    }



    // Update is called once per frame
    void Update()
    {
        if(gameOver){
            LoseGame();
        }
        
        coinText.text = "Coins: " + coins;
        healthText.text = "Health: " + health;
    }
}
