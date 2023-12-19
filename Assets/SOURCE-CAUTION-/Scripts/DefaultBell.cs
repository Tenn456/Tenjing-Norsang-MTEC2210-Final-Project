using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultBell : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clip;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponentInChildren<AudioSource>();
        clip = audioSource.clip;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Bell rung");
            gameManager.pause = true;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(clip);
            Invoke("NextLevel", 3);
        }
        
    }

    private void NextLevel()
    {
        gameManager.NewLevel();
        SceneManager.LoadScene(1);
    }


}
