using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;
    private static Checkpoint instance;
    private AudioSource audioSource;
    private AudioClip clip;
    public bool firstTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (firstTrigger)
            {
                //Saves gamestate
                gameManager.lastPos = transform.position;
                gameManager.lastScore = gameManager.score;
                gameManager.lasttimeCounter = gameManager.timeCounter;
                firstTrigger = false;
                audioSource.PlayOneShot(clip);
                Debug.Log("Checkpoint reached");
            }
            
        }
    }
}
