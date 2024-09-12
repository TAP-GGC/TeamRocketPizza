using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavController : MonoBehaviour
{
    // Load the next scene
    public void GoToScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
