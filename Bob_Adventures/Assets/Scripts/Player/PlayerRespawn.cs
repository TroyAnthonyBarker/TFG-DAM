using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip respawnAudio;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Text txtLives;

    private PlayerInventory playerInventory;
    private Transform currentCheckPoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = FindObjectOfType<UIManager>();
        currentCheckPoint = startPoint;
    }

    private void Update()
    {
        txtLives.text = playerInventory.lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SoundManager.instance.PlaySound(respawnAudio);
            currentCheckPoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.transform.Find("Indicator").gameObject.SetActive(true);
            playerHealth.AddHealth(Mathf.Infinity);
        }
    }

    public void CheckRespawn()
    {
        if (playerInventory.lives <= 0)
        {
            uiManager.GameOver();
            return;
        }

        transform.position = currentCheckPoint.position;
        playerInventory.lives = playerInventory.lives - 1;
        playerHealth.Spawn();
    }
}
