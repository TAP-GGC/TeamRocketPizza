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
    private Animator anim;
    private Text healthText;
    private Text coinText;
    private Text GameState;
    private Text Detail;
    private CanvasGroup _cgroup;
    private CanvasGroup imgGroup;
    private Button butt1;
    private int currentState;
    public int coins;
    public int health;
    private bool gameOver;
    

    private void Awake()
    {
        main = this;
        
    }

    private void Start(){

        currentState = 1;
        Time.timeScale = 0;
        coins = 70;
        health = 20;
        gameOver = false;
        

        
        _cgroup = GameObject.Find("StateMenu").GetComponent<CanvasGroup>();
        imgGroup = GameObject.Find("VirusPanel").GetComponent<CanvasGroup>();
        butt1 = GameObject.Find("BackButton").GetComponent<Button>();

        GameState = GameObject.Find("GameState").GetComponent<Text>();
        Detail = GameObject.Find("DetailText").GetComponent<Text>();
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
        coins += amount/4;
    }

    public void decreaseHealth(int amount){
        health -= amount;
        if(health <= 0){
            gameOver = true;
            
        }
    }

    public void TextSwitch(){
        if(currentState == 0){
            SceneManager.LoadScene("TDGame");
        }
        if(currentState == 1){
            HideAllGroup();
            currentState = 2;
        }
        else if(currentState == 2){
            GameState.text = "";
            ShowDetail();
            currentState = 3;
        }
        else if(currentState == 3){
           HideAllGroup();
        }
        else if(currentState == 4){
            Time.timeScale = 1;
            SceneManager.LoadScene("Desktop 2");
        }
    }

    public void WinGame(){

        GameState.text = "You Win! \nClick button to go back";
        _cgroup.alpha = 1f;
        GameState.fontSize = 100;
        _cgroup.interactable = true;
        Time.timeScale = 0;
        currentState = 4;
    }

    public void LoseGame(){
        
        GameState.text = "Game Over!\nYou Lose\nClick below to Restart";
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        Time.timeScale = 0;

        currentState = 0;
    }
    public void HideAllGroup(){
        GameState.text = "";
        Detail.text = "";
        imgGroup.alpha = 0f;
        _cgroup.alpha = 0f;
        _cgroup.interactable = false;
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

    public void ShowDetail(){
        
        imgGroup.alpha = 1f;
        Detail.text = "worm viruses are malware that self-replicate and spreads itself across your network. Normal malware anti-virus can't detect them. They mainly attack network system and can infect it quickly. \n\n\n Effective defenses: Network Tower. \n Abilities: duplication.";
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
