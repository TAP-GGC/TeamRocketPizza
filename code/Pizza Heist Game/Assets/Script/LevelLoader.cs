using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel(string sceneString){
        Time.timeScale = 1;
        StartCoroutine(TransitionTime(sceneString));

    }

    public IEnumerator TransitionTime(string sceneString){
        // Play Anim
        transition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //Load Scene
        SceneManager.LoadScene(sceneString);
    }
}
