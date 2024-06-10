using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int diamonds { get; private set; }
    public int coins { get; private set; }
    public int skillPoints { get; private set; }
    public int lives { get; set; }

    private void Awake()
    {
        diamonds = 0;
        coins = PlayerPrefs.GetInt("Coins", 0);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);
        lives = PlayerPrefs.GetInt("Lives", 3);
        Save();
    }


    private float Add(float variable, float amount, float min = 0, float max = Mathf.Infinity)
    {
        return Mathf.Clamp(variable + amount, min, max);
    }

    public void AddDiamonds(float _value)
    {
        diamonds = (int) Add(diamonds, _value, max: 3);
    }

    public void AddCoins(float _value)
    {
        coins = (int)Add(coins, _value);
    }

    public void AddSkillPoints(float _value)
    {
        skillPoints = (int)Add(skillPoints, _value);
    }

    public void Save(string levelName)
    {
        Debug.Log(levelName);
        PlayerPrefs.SetInt(levelName, diamonds);
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }
}
