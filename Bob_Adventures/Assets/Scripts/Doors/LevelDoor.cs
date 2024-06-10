using TMPro;
using UnityEngine;

public class levelDoor : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private string levelSceneName;

    [Header("Diamonds")]
    [SerializeField] private GameObject[] diamonds;
    private PlayerInventory inventory;

    [Header("Titulo")]
    [SerializeField] private GameObject doorName;

    private void Awake()
    {
        foreach (var d in diamonds)
            d.SetActive(false);
        doorName.GetComponent<TextMeshPro>().text = levelSceneName;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            ShowDiamonds(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            ShowDiamonds(false);
    }

    private void ShowDiamonds(bool v)
    {
        float hasDiamonds = PlayerPrefs.GetInt(levelSceneName);
        Debug.Log(levelSceneName);
        foreach (var d in diamonds)
            d.SetActive(v);

        foreach (var d in diamonds)
            d.GetComponent<SpriteRenderer>().color = Color.black;

        for (int i = 0; i < hasDiamonds; i++)
        {
            diamonds[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
