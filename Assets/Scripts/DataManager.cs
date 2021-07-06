using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public SaveData saveData;
    public int savedScore;
    public string hsName;
    
    void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
        Load();
    }

    [System.Serializable]
    public class SaveData
    {
        public string plName;
        public int plScore;
        public string plNameHS;
    }

    public void Save()
    {
        saveData = new SaveData();
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            saveData.plScore = MainManager.m_HighScore;
            saveData.plNameHS = MainManager.m_HighScoreName;
        }
        else
        {
            saveData.plName = SettingsManager.Instance.playerName;
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
        Debug.Log("game saved at " + Application.persistentDataPath + "/savedata.json");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded"); 

            MainManager.m_HighScore = data.plScore; 
            MainManager.m_HighScoreName = data.plNameHS;
            savedScore = data.plScore;
            hsName = data.plNameHS;
        }
    }
 
    
}
