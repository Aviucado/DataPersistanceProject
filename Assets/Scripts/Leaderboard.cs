using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text score;
    public static Leaderboard Instance;
    public static PlayerData plDat;
    public List<TMP_Text> scoreTextList = new List<TMP_Text>();
    [SerializeField]
    public static  List<PlayerData> leaderboardList = new List<PlayerData>();

    void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        
        Instance = this;
        DontDestroyOnLoad(Instance);
        //FillList();
    }
    
    void Start()
    {
        //DataManager.Instance.Load();
        plDat = new PlayerData();
        UpdateLeaderBoard();
    }
    void Update() 
    {
        FillList();
    }

    [System.Serializable]
    public class PlayerData
    {
        public string plName;
        public int plScore;
       
    }

    public void UpdateLeaderBoard()
    {
        for(int i = 0; i < scoreTextList.Count; i++)
        {
            if(i < leaderboardList.Count)
            {
                scoreTextList[i].text = leaderboardList[i].plScore.ToString() + " " + leaderboardList[i].plName;
            }
            else
            {
                scoreTextList[i].text = "---";
            }
        }
    }

    public void FillList()
    {
        while(leaderboardList.Count < 5)
        {
            leaderboardList.Add(new PlayerData());
        }
    }
   public void SortScoreList(string name, int score)
    {
        plDat.plName = name;
        plDat.plScore = score;
        DataManager.Instance.Load();
        if(!leaderboardList.Contains(plDat))
        {
            if(leaderboardList.Count <= 5)
            {
                leaderboardList.Add(plDat);
            }
            else
            {
                leaderboardList.RemoveAt(4);
                leaderboardList.Add(plDat);
            }
        }
        
        leaderboardList.Sort(SortScores);
        leaderboardList.Reverse();
    }

    int SortScores(PlayerData p1, PlayerData p2)
    {
        return p1.plScore.CompareTo(p2.plScore);
    }

    public static bool IsNewHighScore(int score)
    {
        if(score > MainManager.m_HighScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
