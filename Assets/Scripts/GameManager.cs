using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// Player Info
    /// </summary>
    public string playerName;
    int playerScore;

    [SerializeField]
    TextMeshProUGUI playerNameText;
    [SerializeField]
    TextMeshProUGUI playerScoreText;

    /// <summary>
    /// Game Scores
    /// </summary>
    public HighScores highScores;

    [SerializeField]
    GameObject highscoresPanel;

    [SerializeField]
    TextMeshProUGUI requiredText;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadData();
    }


    public void inputName(string nameInput)
    {
        if (!string.IsNullOrEmpty(nameInput)) { 
        playerName = nameInput;
        }
    }

    public void StartGame()
    {
        // save name b4 start
        SavePlayerData(playerScore != 0 ? playerScore : 0);
        // move to game scene
        if (!string.IsNullOrEmpty(playerName))
        {
        SceneManager.LoadScene(1);
        }
        requiredText.gameObject.SetActive(true);
        return;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void ViewHighScores()
    {
        highscoresPanel.SetActive(true);
    }

    [Serializable]
    class PlayerData
    {
        public string name;
        public HighscoreEntry score;
    }

    void SavePlayerData(int score)
    {
        PlayerData playerData = new PlayerData();
        playerData.name = playerName;
        playerData.score = new HighscoreEntry { name = playerName, score = score > playerScore ? score: playerScore };

        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

   public class HighScores
    {
        public List<HighscoreEntry> highscoreEntries;
        public int highScore;
        public string highScoreName;
    }

    [Serializable]
   public class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public void AddScore(int score)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = playerName };

        
        SavePlayerData(highscoreEntry.score);
        
        if(highScores == null)
        {
            highScores = new HighScores()
            {
                highscoreEntries = new List<HighscoreEntry>()
            };
        }

        highScores.highscoreEntries.Add(highscoreEntry);
        if(highscoreEntry.score > highScores.highScore)
        {

        highScores.highScore = highscoreEntry.score;
            highScores.highScoreName = highscoreEntry.name;
        }

        string highScoreDataPath = Application.persistentDataPath + "/highscores.json";
        string json = JsonUtility.ToJson(highScores);
        File.WriteAllText(highScoreDataPath, json);

    }

    void LoadData()
    {
        string playerDataPath = Application.persistentDataPath + "/playerData.json";
        string highScoreDataPath = Application.persistentDataPath + "/highscores.json";

        playerNameText.text = "Hi";
        playerScoreText.text = "0";

        if (File.Exists(playerDataPath))
        {
            string json = File.ReadAllText(playerDataPath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            playerName = data.name;
            playerScore = data.score.score;

            playerNameText.text =$"Hi, {playerName}";
            playerScoreText.text = "Your score: " + playerScore;
        }

        if (File.Exists(highScoreDataPath))
        {
            string json = File.ReadAllText(highScoreDataPath);
            HighScores highscores = JsonUtility.FromJson<HighScores>(json);
            highScores = highscores;
        }
    }
}
