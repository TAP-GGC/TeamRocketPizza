using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    private CanvasGroup _cgroup2;

    public int coins;
    public int health;
    private bool gameOver;
    private bool isWarningActive = false;

    private void Awake()
    {
        main = this;
        
    }

    private void Start(){
        Time.timeScale = 0;
        coins = 70;
        health = 20;
        gameOver = false;

        _cgroup2 = GameObject.Find("WarningMsg").GetComponent<CanvasGroup>();
        _cgroup = GameObject.Find("StateMenu").GetComponent<CanvasGroup>();
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

    public void decreaseHealth(int amount){
        health -= amount;
        if(health <= 0){
            gameOver = true;
            
        }
    }

    public void WinGame(){

        GameState.text = "You Win! \nClick button to go back";
        _cgroup.alpha = 1f;
        GameState.fontSize = 100;
        _cgroup.interactable = true;
        Time.timeScale = 0;
        
    }

    public void LoseGame(){
        
        GameState.text = "Game Over!\nYou Lose\n Space to Restart";
        _cgroup.alpha = 1f;
        _cgroup.interactable = true;
        Time.timeScale = 0;

        if(Input.GetButtonDown("Jump")){
            SceneManager.LoadScene("TDGame");
        }
    }
    public void StartGame(){
        if(Input.GetButtonDown("Jump")){
            GameState.text = "";
            _cgroup.alpha = 0f;
            _cgroup.interactable = false;
            Time.timeScale = 1;
        }else{
            return;
        }
    }

    public void Warning()
    {
    // Activate the warning message
        _cgroup2.alpha = 1f;
        Time.timeScale = 0;
        isWarningActive = true; // Set a flag to indicate the warning is active
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver){
            StartGame();
        }
        else{
            LoseGame();
        }
        if (isWarningActive)
        {
            if(Input.GetMouseButtonDown(0)){
                _cgroup2.alpha = 0f;
                Time.timeScale = 1;
                isWarningActive = false; // Reset the flag
            }
            
        }

        coinText.text = "Coins: " + coins;
        healthText.text = "Health: " + health;
    }
}
