using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleMinDuration;
    [SerializeField] private float idleMaxDuration;
    private float idleDuration;
    private float idleTimer;
    private bool changeIdleDuration;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    //References
    private AudioSource source;
    Vector3 previusPosition;


    private void Awake()
    {
        initScale = enemy.localScale;
        previusPosition = enemy.position;
        source = GetComponentInChildren<AudioSource>();
    }
    private void OnDisable()
    {
        anim.SetBool("isMoving", false);
    }

    private void FixedUpdate()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
                changeIdleDuration = true;
            } 
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
                changeIdleDuration = true;
            } 
            else
                DirectionChange();
        }

        if (previusPosition != enemy.position)
            source.UnPause();
        else
            source.Pause();

        previusPosition = enemy.position;

    }

    private void DirectionChange()
    {
        if (changeIdleDuration)
        {
            idleDuration = Random.Range(idleMinDuration, idleMaxDuration);
            changeIdleDuration = false;
        }

        anim.SetBool("isMoving", false);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        anim.SetBool("isMoving", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}