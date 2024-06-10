using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    private float lifeTimeCounter;
    private bool hit;

    // References
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        float moveSpeed = speed * Time.deltaTime;
        transform.Translate(0, -moveSpeed, 0);

        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter > lifeTime )
            Deactivate();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);
        boxCollider.enabled = false;


        Deactivate();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifeTimeCounter = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
