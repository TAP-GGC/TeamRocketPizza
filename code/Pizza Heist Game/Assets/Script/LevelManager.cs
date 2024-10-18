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
    private CanvasGroup wavegroup;
    private CanvasGroup Chatcgroup;
    private CanvasGroup imgGroup;
    public int coins;
    public int health;
    private bool gameOver;
    private GameStateEnum currentState;
    private EnemySpawner enemySpawner;


    [Header("Virus images")]
    [SerializeField] private Image images;
    [SerializeField] private Sprite malware;
    [SerializeField] private Sprite Ransomware;
    [SerializeField] private Sprite Dos;
    [SerializeField] private Sprite Worm;
    [SerializeField] private Sprite Trojan;
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

       

        enemySpawner = GetComponent<EnemySpawner>();
        _cgroup = GameObject.Find("StateMenu").GetComponent<CanvasGroup>();
        Chatcgroup = GameObject.Find("BossChat").GetComponent<CanvasGroup>();
        imgGroup = GameObject.Find("VirusImagePanel").GetComponent<CanvasGroup>();
        wavegroup = GameObject.Find("WaveComplete").GetComponent<CanvasGroup>();
        GameState = GameObject.Find("GameState").GetComponent<Text>();
        
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        coinText = GameObject.Find("CoinsText").GetComponent<Text>();

        imgGroup.blocksRaycasts = false;
        imgGroup.interactable = false;
        currentState = GameStateEnum.Malware;
        Time.timeScale = 0;
        
        gameOver = false;
        
        
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
                Time.timeScale = 1;
                ShowGroup();
                ShowImage(malware,102,5,103);
                currentState = GameStateEnum.Ransomware; // Transition to hide Malware
                break;
            case GameStateEnum.Ransomware:
                HideAllGroup();
                Time.timeScale = 1;
                ShowGroup();
                ShowImage(Ransomware,190,159,38);
                currentState = GameStateEnum.Dos; // Transition to Worm detail
                break;
            case GameStateEnum.Dos:
                HideAllGroup();
                Time.timeScale = 1;
                ShowGroup();
                ShowImage(Dos,65,123,219);
                currentState = GameStateEnum.Worm; // Transition to hide Worm
                break;
            case GameStateEnum.Worm:
                HideAllGroup();
                Time.timeScale = 1;
                ShowGroup();
                ShowImage(Worm,200,12,94);
                currentState = GameStateEnum.Trojan; // Transition to start the game
                break;
            case GameStateEnum.Trojan:
                HideAllGroup();
                Time.timeScale = 1;
                ShowGroup();
                ShowImage(Trojan,251,230,51);
                currentState = GameStateEnum.EndGame; // Transition to hide Worm
                break;
            case GameStateEnum.EndGame:
                Time.timeScale = 1;
                SceneManager.LoadScene("Desktop 2");
                break;
        }
    }

    public void WinGame(){
        HideGroup();
        GameState.text = "You Win! \nClick button to go back";
        _cgroup.alpha = 1f;
        GameState.fontSize = 100;
        _cgroup.interactable = true;
        Time.timeScale = 0;
        currentState = GameStateEnum.EndGame;
    }

    public void LoseGame(){
        HideGroup();
        GameState.text = "Game Over!\nYou Lose\nClick to Restart";
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        _cgroup.blocksRaycasts = true;
        Time.timeScale = 0;
        currentState = GameStateEnum.StartGame;
    }
    public void HideAllGroup(){
        GameState.text = "";
        imgGroup.alpha = 0f;
        _cgroup.alpha = 0f;
        _cgroup.interactable = false;
        _cgroup.blocksRaycasts = false;
        
    }
    public void ShowImage(Sprite sprite, float r, float g, float b)
    {
        images.sprite = sprite;

        // Create a Color using the provided RGB values, with an optional alpha value of 1 (fully opaque)
        Color color = new Color(r / 255f, g / 255f, b / 255f, 1f);

        // Apply the color to the image
        images.color = color;
    }

    public void ShowGroup(){
        imgGroup.alpha = 1f;
        Chatcgroup.alpha = 1f;
        Chatcgroup.interactable = true;
        Chatcgroup.blocksRaycasts = true;
        
    }


    public void HideGroup(){
        imgGroup.alpha = 0f;
        Chatcgroup.alpha = 0f;
        Chatcgroup.interactable = false;
        Chatcgroup.blocksRaycasts = false;
        ;
    }

    public void Warning()
    {
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        _cgroup.blocksRaycasts = true;
        Time.timeScale = 0;
        GameState.fontSize = 120;
        GameState.text = "Warning! A New Virus is Approaching";
        
    }

    public void WaveComplete(){
        wavegroup.alpha = 1f;
        StartCoroutine(waitAndHide());
        
    }

    public IEnumerator waitAndHide(){
        yield return new WaitForSeconds(2f);
        wavegroup.alpha = 0f;
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
