using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public TMP_Text highScoreText;
    public TMP_Text highScoreNameText;
    public GameObject GameOverText;
    public GameObject HighScoreCanvas;
    private bool m_Started = false;
    public static int m_Points;
    public static int m_HighScore;
    public static int m_MinScore = 0;
    public static string m_HighScoreName;
    private bool m_GameOver = false;
    

     void Awake() 
    {
        m_Points=0;
        GenerateBricks();     
    }
    
    void Update()
    {
        highScoreText.text = m_HighScore.ToString();
        highScoreNameText.text =  m_HighScoreName;
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();
                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            GameOverText.SetActive(true);
            bool isHighScore = Leaderboard.IsNewHighScore(m_Points);
            if(isHighScore)
            {
                m_HighScore = m_Points;
                m_HighScoreName = SettingsManager.Instance.playerName;
                GameOver();
                Leaderboard.Instance.SortScoreList(SettingsManager.Instance.playerName, m_HighScore);
                
                isHighScore = false;
            }
            else
            {
                GameOver();
                
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GenerateBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        
        if (Input.GetKey(KeyCode.Space))
        {
            DataManager.Instance.Save();
            GameOverText.SetActive(false);
            StartCoroutine(ReloadDelay());
        }  
    }
    public void RealoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        m_GameOver = false;
        StartCoroutine(GenerateDelay());
        Leaderboard.Instance.UpdateLeaderBoard();
    }

    IEnumerator GenerateDelay()
    {
        yield return new WaitForSeconds(0.2f);
        GenerateBricks();
    }
    IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(0.2f);
        RealoadScene();
    }
}
