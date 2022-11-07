using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoresPanel : MonoBehaviour
{
    Transform entryContainer;
    Transform entryTemplate;
    List<Transform> highScoresList;

    private void Awake()
    {
        entryContainer = transform.Find("Content").Find("HighScoresContainer");
        entryTemplate = entryContainer.Find("HighScoresTemplate");

        entryTemplate.gameObject.SetActive(false);
        GameManager.HighScores highScores = GameManager.instance.highScores;

        for(int i = 0; i < highScores.highscoreEntries.Count; i++)
        {
            for(int j = i +1;j < highScores.highscoreEntries.Count; j++)
            {
                if (highScores.highscoreEntries[i].score > highScores.highscoreEntries[j].score)
                {
                    GameManager.HighscoreEntry tmp = highScores.highscoreEntries[i];
                    highScores.highscoreEntries[i] = highScores.highscoreEntries[j];
                    highScores.highscoreEntries[j] = tmp;
                }
            }
        }

        highScoresList = new List<Transform>();
        foreach(GameManager.HighscoreEntry highscoreEntry in highScores.highscoreEntries)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highScoresList);
        }
        
    }

    void CreateHighscoreEntryTransform(GameManager.HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;

        entryTransform.Find("PositionText").GetComponent<TextMeshProUGUI>().text = rank.ToString();
        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.score.ToString();
        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.name.ToString();

        transformList.Add(entryTransform);
    }
}
