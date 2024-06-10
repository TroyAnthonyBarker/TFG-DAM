using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // References
    public static bool isPaused { get; private set; }
    private bool levelPassed = false;

    [Header("Click")]
    [SerializeField] private AudioClip clickSound;

    [Header("Game Win")]
    [SerializeField] private GameObject levelPassedScreen;
    [SerializeField] private AudioClip levelPassedSound;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Game")]
    [SerializeField] private GameObject pauseGameScreen;
    [SerializeField] private AudioClip pauseGameSound;

    [Header("Timer")]
    [SerializeField] private Text txtTimer;
    private float second;
    private int hour, minute;
    private string format = "00";

    [Header("Scoreboard")]
    [SerializeField] private GameObject scoreboardScreen;

    public static string gameTime { get; private set; }

    private void Awake()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (pauseGameScreen != null)
            pauseGameScreen.SetActive(false);
        if (levelPassedScreen != null)
            levelPassedScreen.SetActive(false);
        if (scoreboardScreen != null)
            scoreboardScreen.SetActive(false);
    }

    private void Update()
    {
        if (txtTimer == null) return;

        second += Time.deltaTime;
        if (second >= 60)
        {
            minute++;
            second = 0;
        }

        if (minute >= 60)
        {
            hour++;
            minute = 0;
        }

        gameTime = hour.ToString(format) + ":" + minute.ToString(format) + ":" + second.ToString(format);
        txtTimer.text = gameTime;
    }

    private void StopTime(bool stop)
    {
        if (stop)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    #region Commun Options

    public void Continue()
    {
        SoundManager.instance.PlaySound(clickSound);
        Pause(!isPaused);
    }
    public void Restart()
    {
        SoundManager.instance.PlaySound(clickSound);
        StopTime(false);
        levelPassed = false;
        second = 0;
        minute = 0;
        hour = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WorldMenu()
    {
        SoundManager.instance.PlaySound(clickSound);
        string world = SceneManager.GetActiveScene().name.Split("-")[0];
        StopTime(false);
        SceneManager.LoadScene(world);
    }

    public void WorldMenu(string SceneName)
    {
        SoundManager.instance.PlaySound(clickSound);
        StopTime(false);
        SceneManager.LoadScene(SceneName);
    }

    public void MainMenu()
    {
        SoundManager.instance.PlaySound(clickSound);
        StopTime(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        SoundManager.instance.PlaySound(clickSound);
        Application.Quit();
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }
    #endregion

    #region Level Passed
    public void LevelPassed()
    {
        levelPassedScreen.SetActive(true);
        SoundManager.instance.PlaySound(levelPassedSound);
        levelPassed = true;
        Pause(true);
        playerInventory.Save(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Pause
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (levelPassed) return;
        if (context.performed)
        {
            Pause(!pauseGameScreen.activeInHierarchy);
        }
    }

    private void Pause(bool value)
    {
        isPaused = value;
        if (!levelPassed)
            pauseGameScreen.SetActive(isPaused);

        StopTime(isPaused);
    }

    #endregion

    #region Scoreboard
    public void Scoreboard(bool isActive)
    {
        scoreboardScreen.SetActive(isActive);
    }
    #endregion
}
