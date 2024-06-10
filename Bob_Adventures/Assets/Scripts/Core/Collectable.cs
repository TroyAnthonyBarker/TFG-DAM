using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    protected delegate void Func();
    private bool isPlayer;
    [SerializeField] private AudioClip pickupSound;

    private void Awake()
    {
        isPlayer = false;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayer = true;
            SoundManager.instance.PlaySound(pickupSound);
            gameObject.SetActive(false);
        } else
            isPlayer = false;
    }

    protected void Do(Func func)
    {
        if (isPlayer)
            func();
    }
}
