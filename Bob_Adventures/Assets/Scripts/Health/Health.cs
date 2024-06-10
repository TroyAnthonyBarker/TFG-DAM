using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float initialHealth;
    [SerializeField] private bool godMode;
    public float currentHealth {  get; private set; }
    private bool dead;

    [Header("IFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float numberOfFlashes;
    private readonly int playerLayer = 8;
    private readonly int enemyLayer = 9;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("SFX")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Objects")]
    [SerializeField] private GameObject[] deactivateOnDeath;
    [SerializeField] private GameObject[] activateOnDeath;

    // References
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        currentHealth = initialHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (deactivateOnDeath != null)
        {
            foreach (GameObject component in deactivateOnDeath)
            {
                component.SetActive(true);
            }
        }
        if (activateOnDeath != null)
        {
            foreach (GameObject component in activateOnDeath)
            {
                component.SetActive(false);
            }
        }
    }


    public void TakeDamage(float _damage)
    {
        if (godMode) return;
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, initialHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);
            StartCoroutine(Invunerability());
        } else
        {
            if (!dead)
            {
                if (GetComponentInParent<Rigidbody2D>() != null)
                    GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;

                foreach(Behaviour component in components)
                    component.enabled = false;

                animator.SetTrigger("Die");
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
            
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, initialHealth);
    }

    public void Spawn()
    {
        dead = false;
        AddHealth(initialHealth);
        animator.ResetTrigger("Die");
        animator.Play("Movement");
        StartCoroutine(Invunerability());


        foreach (Behaviour component in components)
            component.enabled = true;
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        if (deactivateOnDeath != null)
        {
            foreach (GameObject component in deactivateOnDeath)
            {
                component.SetActive(false);
            }
        }
        if (activateOnDeath != null) { 
            foreach (GameObject component in activateOnDeath)
            {
                component.SetActive(true);
            }
        }
    }
}
