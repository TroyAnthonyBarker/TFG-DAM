using UnityEngine;
using UnityEngine.InputSystem;

public class MeleAttack : MonoBehaviour
{
    // References
    private Animator Animator;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("SFX")]
    [SerializeField] private AudioClip attackClip;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    private Health enemyHealth;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;
    private float coolDownTimer = Mathf.Infinity;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (coolDownTimer >= attackCooldown)
        {
            SoundManager.instance.PlaySound(attackClip);
            coolDownTimer = 0;
            Animator.SetTrigger("Attack");
        }
    }

    private void DamegeEnemy()
    {
        // If the enemy is in sight hit
        if (EnemyInSight())
            enemyHealth.TakeDamage(damage);
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
}
