using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource2;

    Scene currentScene;
    string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "TutorialLevel")
        {
            audioSource.Play();
        }
        if (sceneName == "Level 1")
        {
            audioSource2.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
