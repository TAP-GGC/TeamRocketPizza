using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button ReplayButton;
    public Button ReturnButton;
    public LevelLoader transitionRef;

    public string returnSceneName;



    void Start()
    {
        
        AddListenersToButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddListenersToButtons()
    {
        // Set up the button listeners
        ReplayButton.onClick.AddListener(OnReplayButtonClicked);
        ReturnButton.onClick.AddListener(OnReturnButtonClicked);
        Debug.Log("Listeners added");
    }





    public void OnReplayButtonClicked()
    {
        // Load the game scene
        Debug.Log("Replay button clicked");
        Time.timeScale = 1;
        transitionRef.LoadNextLevel(SceneManager.GetActiveScene().name);
    }

    public void OnReturnButtonClicked()
    {
        // Load the main menu scene
        Debug.Log("Return button clicked");
        if (returnSceneName != null)
        {
            Time.timeScale = 1;
            transitionRef.LoadNextLevel(returnSceneName);
        }
        else
        {
            Debug.Log("No return scene name set");
        }
    }


}
