using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private Type doorType;
    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip doorSound;



    private UIManager _manager;
    private void Awake()
    {
        _manager = FindObjectOfType<UIManager>();
    }

    public void Run()
    {
        SoundManager.instance.PlaySound(doorSound);
        StartCoroutine(WaitForSound());
        switch (doorType)
        {
            case Type.ExitLevel:
                {
                    ExitLevel();
                    break;
                }
            case Type.Load:
                {
                    LoadScene();
                    break;
                }
        }
    }

    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(doorSound.length);
    }

    private void ExitLevel()
    {
        _manager.LevelPassed();
    }

    private void LoadScene()
    {
        Debug.Log("Loading...");
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Loaded");
    }

    private enum Type
    {
        ExitLevel,
        Load
    }
}