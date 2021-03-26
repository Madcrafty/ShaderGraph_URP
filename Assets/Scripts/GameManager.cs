using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    //public TextMeshProUGUI scoreBoard;
    //public TextMeshProUGUI FPS;
    //public float m_refreshTime = 0.5f;
    //public bool debuging;

    private string scoreBoardBaseText;
    private string fpsBaseText;
    private int score;
    private int winScreenIndex = 2;
    private int gameOverScreenIndex = 3;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        //scoreBoardBaseText = scoreBoard.text;
        //fpsBaseText = FPS.text;
        SceneManager.activeSceneChanged += ChangedActiveScene;
        DontDestroyOnLoad(this);
        score = 0;
        //UpdateScoreBoard();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void ChangedActiveScene(Scene current, Scene next)
    {
        //scoreBoard = GameObject.Find("ScoreBoard").GetComponent<TextMeshProUGUI>();
        //scoreBoardBaseText = scoreBoard.text;
        //UpdateScoreBoard();
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        Destroy(gameObject);
        //if (next.name != "00 Starter Scene")
        //{
        //    Destroy(gameObject);
        //    Destroy(this);
        //}
    }

    // Update is called once per frame
    //void Update()
    //{
        //if (m_timeCounter < m_refreshTime)
        //{
        //    m_timeCounter += Time.deltaTime;
        //    m_frameCounter++;
        //}
        //else
        //{
        //    //This code will break if you set your m_refreshTime to 0, which makes no sense.
        //    m_lastFramerate = (float)m_frameCounter / m_timeCounter;
        //    m_frameCounter = 0;
        //    m_timeCounter = 0.0f;
        //    FPS.text = fpsBaseText + (int)m_lastFramerate;
        //}
    //}

    //public void AddScore(int ammount)
    //{
    //    score += ammount;
    //    UpdateScoreBoard();
    //}
    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(gameOverScreenIndex);
    }
    public void GameWon()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(winScreenIndex);
    }
    //private void UpdateScoreBoard()
    //{
    //    scoreBoard.text = scoreBoardBaseText + score;
    //}
}
