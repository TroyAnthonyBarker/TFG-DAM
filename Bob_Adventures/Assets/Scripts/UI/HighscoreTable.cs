using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : DBManager {

    private Transform entryContainer;
    private Transform entryTemplate;
    private Text levelText;
    private List<Transform> scoreEntryTransformList;

    private Highscores highscores;

    private void Awake() {
        entryContainer = transform.Find("scoreEntryContainer");
        entryTemplate = entryContainer.Find("scoreEntryTemplate");
        
        levelText = transform.Find("levelText").GetComponent<Text>();

        levelText.text = sceneName.Replace("_", " ").ToUpper();

        entryTemplate.gameObject.SetActive(false);

        UpdateTable();
    }

    public void UpdateTable()
    {
        highscores = GetHighscores();

        for (int i = 0; i < highscores.scoreEntryList.Count; i++)
        {
            for (int j = 0; j < highscores.scoreEntryList.Count; j++)
            {
                if (DateTime.Parse(highscores.scoreEntryList[i].Time) < DateTime.Parse(highscores.scoreEntryList[j].Time))
                {
                    ScoreEntry scoreEntry = highscores.scoreEntryList[i];
                    highscores.scoreEntryList[i] = highscores.scoreEntryList[j];
                    highscores.scoreEntryList[j] = scoreEntry;
                }
            }
        }

        scoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < highscores.scoreEntryList.Count && i < 10; i++)
        {
            CreateScoreEntryTransform(highscores.scoreEntryList[i], entryContainer, scoreEntryTransformList);
        }
    }

    private void CreateScoreEntryTransform(ScoreEntry scoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string time = scoreEntry.Time;

        entryTransform.Find("timeText").GetComponent<Text>().text = time.ToString();

        string name = scoreEntry.Name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("timeText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank) {
        default:
            entryTransform.Find("trophy").gameObject.SetActive(false);
            break;
        case 1:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
            break;
        case 2:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
            break;
        case 3:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
            break;

        }

        transformList.Add(entryTransform);
    }

}
