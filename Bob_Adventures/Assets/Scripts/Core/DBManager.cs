using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DBManager : MonoBehaviour
{
    private static HttpClient client = new HttpClient();

    private string BD_baseName = "https://bobs-adventures-default-rtdb.firebaseio.com";
    

    [Header("Publish new Score")]
    [SerializeField] private Text nameTXT;
    [SerializeField] private Text timerTXT;

    [Header("Get")]
    [SerializeField] private protected string sceneName;

    public void PostScore()
    {
        ScoreEntry score = new ScoreEntry(){ Name = nameTXT.text, Time = timerTXT.text };

        Scene currentScene = SceneManager.GetActiveScene();

        string mundo = currentScene.name.Split("-")[0].ToUpper();
        string nivel = currentScene.name.Split("-")[1].Split("_")[0].ToUpper();

        Task.Run(() => CreateProductAsync(score, mundo, nivel)).Wait();
    }

    public Highscores GetHighscores()
    {
        return Task.Run(() => GetProductsAsync(sceneName)).Result;
    }

    private async Task<Uri> CreateProductAsync(ScoreEntry score, string mundo, string nivel)
    {
        // / M1 / L1 .json
        string bd = BD_baseName + "/" + mundo + "/" + nivel + ".json";

        string str = JsonUtility.ToJson(score);

        HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(bd, content);

        response.EnsureSuccessStatusCode();

        Debug.Log(response.StatusCode);
        Debug.Log(response.Content);

        // return URI of the created resource.
        return response.Headers.Location;
    }

    private async Task<Highscores> GetProductsAsync(string sceneName)
    {
        string mundo = sceneName.Split("-")[0];
        string nivel = sceneName.Split("-")[1].Split("_")[0];
        string bd = BD_baseName + "/" + mundo + "/" + nivel + ".json";

        HttpResponseMessage response = await client.GetAsync(
            bd);

        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        string[] data = json.Split("},");
        StringBuilder sb = new StringBuilder();

        foreach (string s in data)
        {
            string temp = s.Split(":{")[1];
            if (!temp.EndsWith("}"))
                temp = temp + "}";
            else if (temp.EndsWith("}}"))
                temp.Replace("}}", "}");

            if (!temp.StartsWith("{"))
                temp = "{" + temp;

            sb.Append(temp + ",");
        }
        
        string dataList = sb.ToString();
        string scoreEntries = "{\"scoreEntryList\":[" + dataList.Substring(0, dataList.Length - 2) + "]}";

        Highscores highscores = JsonUtility.FromJson<Highscores>(scoreEntries);

        // return URI of the created resource.
        return highscores;
    }
}


public class Highscores
{
    public List<ScoreEntry> scoreEntryList;
}
/*
[Serializable]
public class ScoreEntry
{
    public string id;
    public ScoreEntryData data;
}
*/
[Serializable]
public class ScoreEntry
{
    public string Name;
    public string Time;
}

