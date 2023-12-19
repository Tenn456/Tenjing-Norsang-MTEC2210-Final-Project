using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float timeCounter;
    public int minutes;
    public int seconds;
    public float lasttimeCounter;
    string timeFormat = "{0:00}:{1:00}";

    public TextMeshProUGUI scoreText;
    public int score;
    public int lastScore;
    public int scoreSince;

    public bool pause;

    private static GameManager instance;
    public Vector2 lastPos;

    private void Awake()
    {
        //makes it so there arent multiple game managers after multiple deaths
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!pause)
        {
            timeCounter += Time.deltaTime;
            minutes = Mathf.FloorToInt(timeCounter / 60f);
            seconds = Mathf.FloorToInt(timeCounter - minutes * 60);
        }

        timeText.text = "Time: " + string.Format(timeFormat, minutes, seconds);

        scoreText.text = "Score: " + score;
    }

    public void resetScore()
    {
        score -= scoreSince;
        scoreSince = 0;
        score = lastScore;
    }

    public void resetTime()
    {
        timeCounter = lasttimeCounter;
    }

    public void NewLevel()
    {
        lastPos = new Vector2(0, 0);
        score = 0;
        lastScore = 0;
        scoreSince = 0;
        timeCounter = 0;
        lasttimeCounter = 0;
        minutes = 0;
        seconds = 0;
        pause = false;
    }
}
